using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace FabToolKit.Tkx.Sentinel.Rest;

public class PostJobDTO
{
    [JsonPropertyName("@LABEL_NAME")]
    public string LabelName { get; set; }

    [JsonPropertyName("@PRINTER_NAME")]
    public string PrinterName { get; set; }

    [JsonPropertyName("@JOB_NAME")]
    public string? JobName { get; set; }

    [JsonPropertyName("@PRINT_QUANTITY")]
    public int LabelQuantity { get; set; }

    [JsonPropertyName("@EMAIL_RECIPIENTS")]
    public string? EmailRecipients { get; set; }

    [JsonPropertyName("@EMAIL_SUBJECT")]
    public string? EmailSubject { get; set; }

    [JsonPropertyName("@PRINT_PDF")]
    public string? PdfPrintPath { get; set; }

    [JsonPropertyName("@PAGE_BREAK")]
    public int? PageBreak { get
        {
            return PdfPrintPath is not null ? 1 : null; 
        }
    } 

    [JsonExtensionData]
    public Dictionary<string, object> LabelVariables { get; set; } = new();

    public PostJobDTO()
    {
        
    }

    public PostJobDTO(Dictionary<string,object> labVars)
    {
        LabelVariables = labVars;
    }

    public PostJobDTO(NameValueCollection labVars)
    {
        foreach (var item in labVars.AllKeys)
        {
            string key = item;
            string value = labVars[key];

            this.LabelVariables.Add(key, value);
        }
    }

    public PostJobDTO(string labelName, string printerName, int printQuantity)
    {
        LabelName = labelName;
        PrinterName = printerName;
        LabelQuantity = printQuantity;
    }

    public PostJobDTO(string labelName, string printerName, int printQuantity, Dictionary<string,object> labVars) : this(labVars)
    {
        LabelName = labelName;
        PrinterName = printerName;
        LabelQuantity = printQuantity;
    }

    public PostJobDTO(string labelName, string printerName, int printQuantity, NameValueCollection labVars) : this(labVars)
    {
        LabelName = labelName;
        PrinterName = printerName;
        LabelQuantity = printQuantity;
    }

    public PostJobDTO(string labelName, string printerName, int printQuantity, Dictionary<string, object> labVars, string pdfPath) : this(labelName, printerName, printQuantity, labVars)
    {
        this.PdfPrintPath = pdfPath;
    }

    public PostJobDTO(string labelName, string printerName, int printQuantity, NameValueCollection labVars, string pdfPath) : this(labelName, printerName, printQuantity, labVars)
    {
        this.PdfPrintPath = pdfPath;
    }

    public PostJobDTO(string labelName, string printerName, int printQuantity, Dictionary<string, object> labVars, string pdfPath, string emailRecipients, string emailSubject) 
            : this(labelName, printerName, printQuantity, labVars, pdfPath)
    {
        this.EmailRecipients = emailRecipients;
        this.EmailSubject = emailSubject;
    }
}
