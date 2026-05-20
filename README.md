# PDFlat
Flatten those PDFs

## Overview

**PDFlat** is a .NET 8 console application that flattens multi-layer PDFs so they render correctly in all PDF viewers (including Microsoft Edge).

It processes every `.pdf` file in a given folder and writes the flattened copies to a `pdflat` sub-folder, leaving the originals unchanged.

### What "flattening" does

Multi-layer PDFs use _Optional Content Groups_ (OCG) to control layer visibility.  Some viewers (notably the Edge built-in PDF renderer) do not handle all OCG configurations reliably.

PDFlat:
1. **Makes all layers unconditionally visible** – sets every OCG layer to ON and clears the default "OFF" list so no layer is ever hidden.
2. **Flattens AcroForm fields** – bakes form-field appearances (text boxes, checkboxes, signatures, etc.) directly into the page content.

The result is a PDF that renders identically across all compliant viewers, with no layer panel and no interactive form fields.

## Requirements

* [.NET 8 SDK](https://dotnet.microsoft.com/download) or later

## Building

```bash
dotnet build PDFlat/PDFlat.csproj -c Release
```

## Usage

```
PDFlat [folder]
```

| Argument | Description |
|----------|-------------|
| `folder` | Path to the folder containing PDF files. Defaults to the **current directory** if omitted. |

Flattened PDFs are written to `<folder>/pdflat/`, which is created automatically.

### Examples

```bash
# Flatten all PDFs in the current directory
dotnet run --project PDFlat/PDFlat.csproj -c Release

# Flatten all PDFs in a specific folder
dotnet run --project PDFlat/PDFlat.csproj -c Release -- "C:\MyDocuments\PDFs"

# Run the compiled binary directly
PDFlat.exe "C:\MyDocuments\PDFs"
```

### Sample output

```
Input  folder : C:\MyDocuments\PDFs
Output folder : C:\MyDocuments\PDFs\pdflat
PDFs found    : 3

  document1.pdf ... done
  document2.pdf ... done
  document3.pdf ... FAILED (...)

Finished: 2 succeeded, 1 failed.
```

### Owner-restricted PDFs

Some PDFs are set to "owner password" mode (marked as print-only or read-only by the creator, but still viewable without a password). PDFlat processes these files by default since you are explicitly choosing to flatten documents you have legitimate access to.


[AGPL-3.0](LICENSE) — this project uses [iText7](https://github.com/itext/itext7-dotnet) which is licensed under AGPL-3.0.
