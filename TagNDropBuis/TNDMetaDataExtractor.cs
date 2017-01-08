using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropLibrary;

namespace TagNDropBuis {
    public class TNDMetaDataExtractor {
        public static bool ConvertFileContents(TNDDropItem fc) {
            if (fc.Content == null) { return false; }
            if (fc.Name.EndsWith(".EML", StringComparison.OrdinalIgnoreCase)) {
                return ConvertFileContentsEML(fc);
            }
            if (fc.Name.EndsWith(".MSG", StringComparison.OrdinalIgnoreCase)) {
                return ConvertFileContentsMSG(fc);
            }
            if (fc.Name.EndsWith(".DOC", StringComparison.OrdinalIgnoreCase)) {
                return ConvertFileContentsDOC(fc);
            }
            return false;
        }

        public static bool ConvertFileContentsEML(TNDDropItem fc) {
            if (fc.Content == null) { return false; }
            var message = MsgReader.Mime.Message.Load(new MemoryStream(fc.Content));
            fc.SetProperty("From", message.Headers?.Sender?.MailAddress?.Address);
            fc.SetProperty("Subject", message.Headers?.Subject);
            fc.SetProperty("Text", message.TextBody.GetBodyAsText());
            return true;
        }

        public static bool ConvertFileContentsMSG(TNDDropItem fc) {
            if (fc.Content == null) { return false; }
            using (var message = new MsgReader.Outlook.Storage.Message(new MemoryStream(fc.Content))) {
                fc.SetProperty("From", message.Sender?.Email);
                fc.SetProperty("Subject", message.Subject);
                fc.SetProperty("Text", message.BodyText);

                // switch (message.Type)
                // {
                // case MsgReader.Outlook.Storage.Message.MessageType.Email:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailSms:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailNonDeliveryReport:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailDeliveryReport:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailDelayedDeliveryReport:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailReadReceipt:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailNonReadReceipt:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailEncryptedAndMaybeSigned:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailEncryptedAndMaybeSignedNonDelivery:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailEncryptedAndMaybeSignedDelivery:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailClearSignedReadReceipt:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailClearSignedNonDelivery:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailClearSignedDelivery:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailBmaStub:
                // case MsgReader.Outlook.Storage.Message.MessageType.CiscoUnityVoiceMessage:
                // case MsgReader.Outlook.Storage.Message.MessageType.EmailClearSigned:
                //    return message.Headers.From
                //        WriteMsgEmail(message, outputFolder, hyperlinks).ToArray();

                // case Storage.Message.MessageType.Appointment:
                // case Storage.Message.MessageType.AppointmentNotification:
                // case Storage.Message.MessageType.AppointmentSchedule:
                // case Storage.Message.MessageType.AppointmentRequest:
                // case Storage.Message.MessageType.AppointmentRequestNonDelivery:
                // case Storage.Message.MessageType.AppointmentResponse:
                // case Storage.Message.MessageType.AppointmentResponsePositive:
                // case Storage.Message.MessageType.AppointmentResponsePositiveNonDelivery:
                // case Storage.Message.MessageType.AppointmentResponseNegative:
                // case Storage.Message.MessageType.AppointmentResponseNegativeNonDelivery:
                // case Storage.Message.MessageType.AppointmentResponseTentative:
                // case Storage.Message.MessageType.AppointmentResponseTentativeNonDelivery:
                //    return WriteMsgAppointment(message, outputFolder, hyperlinks).ToArray();

                // case Storage.Message.MessageType.Contact:
                //    return WriteMsgContact(message, outputFolder, hyperlinks).ToArray();

                // case Storage.Message.MessageType.Task:
                // case Storage.Message.MessageType.TaskRequestAccept:
                // case Storage.Message.MessageType.TaskRequestDecline:
                // case Storage.Message.MessageType.TaskRequestUpdate:
                //    return WriteMsgTask(message, outputFolder, hyperlinks).ToArray();

                // case Storage.Message.MessageType.StickyNote:
                //    return WriteMsgStickyNote(message, outputFolder).ToArray();

                // case Storage.Message.MessageType.Unknown:
                //    throw new MRFileTypeNotSupported("Unsupported message type");
                // }
                return true;
            }
        }

        public static bool ConvertFileContentsDOC(TNDDropItem fc) {
            //if (fc.Content == null) { return false; }
            //var ps = new NPOI.HPSF.PropertySet(fc.Content);
            //var summaryInfo = new NPOI.HPSF.SummaryInformation(ps);
            //var documentSummaryInformation = new NPOI.HPSF.DocumentSummaryInformation(ps);


            //var filesystem = new NPOI.POIFS.FileSystem.POIFSFileSystem(fc.Content);
            //var documentProps = (NPOI.POIFS.FileSystem.DocumentEntry)filesystem.Root.GetEntry("WordDocument");
            //var mainStream = new byte[documentProps.Size];
            //filesystem.CreateDocumentInputStream("WordDocument").Read(mainStream);
            //var fib = new NPOI.HWPF.Model.FileInformationBlock(mainStream);

            //Console.WriteLine(summaryInfo.ApplicationName);
            //Console.WriteLine(summaryInfo.Author);
            //Console.WriteLine(summaryInfo.Comments);
            //Console.WriteLine(summaryInfo.CharCount);
            //Console.WriteLine(summaryInfo.EditTime);
            //Console.WriteLine(summaryInfo.Keywords);
            //Console.WriteLine(summaryInfo.LastAuthor);
            //Console.WriteLine(summaryInfo.PageCount);
            //Console.WriteLine(summaryInfo.RevNumber);
            //Console.WriteLine(summaryInfo.Security);
            //Console.WriteLine(summaryInfo.Subject);
            //Console.WriteLine(summaryInfo.Template);

            return true;
        }
        public static bool ConvertFileContentsDOCX(TNDDropItem fc) {
            if (fc.Content == null) { return false; }
            var doc = new NPOI.XWPF.UserModel.XWPFDocument(new MemoryStream(fc.Content));
            var properties = doc.GetProperties();

            //var ps = new NPOI.OpenXml4Net.. .PropertySet(fc.Content);
            //var summaryInfo = new NPOI.HPSF.SummaryInformation(ps);
            //var x = new NPOI.HPSF.DocumentSummaryInformation(ps);
            //Console.WriteLine(summaryInfo.ApplicationName);
            //Console.WriteLine(summaryInfo.Author);
            //Console.WriteLine(summaryInfo.Comments);
            //Console.WriteLine(summaryInfo.CharCount);
            //Console.WriteLine(summaryInfo.EditTime);
            //Console.WriteLine(summaryInfo.Keywords);
            //Console.WriteLine(summaryInfo.LastAuthor);
            //Console.WriteLine(summaryInfo.PageCount);
            //Console.WriteLine(summaryInfo.RevNumber);
            //Console.WriteLine(summaryInfo.Security);
            //Console.WriteLine(summaryInfo.Subject);
            //Console.WriteLine(summaryInfo.Template);

            return true;
        }
    }
}
