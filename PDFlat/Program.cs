using PDFlat;

// Determine input folder from first argument, or fall back to current directory
string inputFolder = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();

if (!Directory.Exists(inputFolder))
{
    Console.Error.WriteLine($"Error: Folder '{inputFolder}' does not exist.");
    Environment.Exit(1);
}

string outputFolder = Path.Combine(inputFolder, "pdflat");

var pdfFiles = Directory.GetFiles(inputFolder, "*.pdf", new EnumerationOptions
{
    MatchCasing = MatchCasing.CaseInsensitive,
    RecurseSubdirectories = false
});

if (pdfFiles.Length == 0)
{
    Console.WriteLine("No PDF files found in the specified folder.");
    return;
}

Directory.CreateDirectory(outputFolder);

Console.WriteLine($"Input  folder : {inputFolder}");
Console.WriteLine($"Output folder : {outputFolder}");
Console.WriteLine($"PDFs found    : {pdfFiles.Length}");
Console.WriteLine();

int succeeded = 0;
int failed = 0;

foreach (var inputPath in pdfFiles)
{
    string fileName = Path.GetFileName(inputPath);
    string outputPath = Path.Combine(outputFolder, fileName);

    Console.Write($"  {fileName} ... ");

    try
    {
        PdfFlattener.Flatten(inputPath, outputPath);
        Console.WriteLine("done");
        succeeded++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"FAILED ({ex.Message})");
        failed++;
    }
}

Console.WriteLine();
Console.WriteLine($"Finished: {succeeded} succeeded, {failed} failed.");
