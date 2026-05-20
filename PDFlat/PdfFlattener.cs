using iText.Forms;
using iText.Kernel.Pdf;

namespace PDFlat;

/// <summary>
/// Flattens a PDF by making all optional-content layers visible and baking
/// annotation/form-field appearances into the page content.
/// </summary>
internal static class PdfFlattener
{
    /// <summary>
    /// Reads <paramref name="inputPath"/>, flattens its layers and form fields,
    /// and writes the result to <paramref name="outputPath"/>.
    /// </summary>
    public static void Flatten(string inputPath, string outputPath)
    {
        using var reader = new PdfReader(inputPath);

        // Allow reading PDFs that use encryption without requiring a password
        reader.SetUnethicalReading(true);

        using var writer = new PdfWriter(outputPath);
        using var doc = new PdfDocument(reader, writer);

        // ── 1. Flatten AcroForm fields (widgets, digital signatures, etc.) ────────
        var form = PdfAcroForm.GetAcroForm(doc, createIfNotExist: false);
        form?.FlattenFields();

        // ── 2. Make every Optional Content Group (layer) visible ─────────────────
        //    For each layer: call SetOn(true) to mark it visible.
        //    Also clear the "OFF" array and "AS" (usage-application) entries from the
        //    default configuration dictionary so that no layer is hidden by default.
        //    Finally, mark the OCProperties dictionary as modified so iText7 writes
        //    the updated version to the output file.
        var ocProps = doc.GetCatalog().GetOCProperties(false);
        if (ocProps != null)
        {
            foreach (var layer in ocProps.GetLayers())
            {
                layer.SetOn(true);
            }

            var ocDict = ocProps.GetPdfObject();
            var defaultConfig = ocDict.GetAsDictionary(new PdfName("D"));
            if (defaultConfig != null)
            {
                defaultConfig.Remove(new PdfName("OFF"));
                defaultConfig.Remove(new PdfName("AS"));
                defaultConfig.SetModified();
            }

            ocDict.SetModified();
        }
    }
}
