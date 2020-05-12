using Microsoft.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
//using System.Web.Security;
using System.IO;

namespace ITC
{
    public class sentEmail
    {
        //;chaiyutc@meyer-mil.com
        public string from = "swadmin@meyer-mil.com";
        public string host = "http://10.100.47.10";
        //public string mailto = "sontorns@meyer-mil.com";
        //public string mailcc = "aofhardcore@gmail.com;sontorn1@gmail.com;chaiyutc@meyer-mil.com";
        public string mailto = "";
        public string mailcc = "";
        public SqlConnection Conn = new SqlConnection();

        public string SendEMailTo(string strFrom, string fromDisplay, string strTo, string strBcc, string strCC, string strSubject, string strBody, bool HTMLBody, string strAttachment = "")
        {
            try
            {
                MailMessage MailMsg = new MailMessage();
                MailMsg.From = new MailAddress(strFrom, fromDisplay);

                if (!string.IsNullOrEmpty(strTo))
                {
                    string[] strToArray = null;
                    strToArray = strTo.Split(';');
                    foreach (string i in strToArray)
                    {
                        MailMsg.To.Add(new MailAddress(i));
                    }
                }
                else
                {
                    return "error; No To Adress Specified";
                }

                if (!string.IsNullOrEmpty(strCC))
                {
                    string[] strccArray = null;
                    strccArray = strCC.Split(';');
                    foreach (string i in strccArray)
                    {
                        MailMsg.CC.Add(new MailAddress(i));
                    }
                }

                if (!string.IsNullOrEmpty(strBcc))
                {
                    string[] strBccArray = null;
                    strBccArray = strBcc.Split(';');
                    foreach (string i in strBccArray)
                    {
                        MailMsg.Bcc.Add(new MailAddress(i));
                    }
                }

                MailMsg.Subject = strSubject;
                MailMsg.Body = strBody;
                if (HTMLBody)
                {
                    MailMsg.IsBodyHtml = true;
                }
                else
                {
                    MailMsg.IsBodyHtml = false;
                }

                //Set attachment file as array
                if (!string.IsNullOrEmpty(strAttachment))
                {
                    string[] strAttachmentArray = null;
                    strAttachmentArray = strAttachment.Split(';');
                    int AttachmentInt = strAttachmentArray.Length;
                    int AttachmentCount = 0;
                    while (AttachmentCount < AttachmentInt)
                    {
                        MailMsg.Attachments.Add(new Attachment(strAttachmentArray[AttachmentCount]));
                        AttachmentCount = AttachmentCount + 1;
                    }
                }

                //Direct send email to Gmail ==========================================
                SmtpClient mSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                //GMail SMTP Host & port / Mail Sever IP address
                try
                {
                    var _with1 = MailMsg;
                    _with1.SubjectEncoding = System.Text.Encoding.UTF8;
                    _with1.BodyEncoding = System.Text.Encoding.UTF8;
                    _with1.Priority = MailPriority.High;
                    var _with2 = mSmtpClient;
                    _with2.EnableSsl = true;
                    _with2.UseDefaultCredentials = false;
                    _with2.Credentials = new System.Net.NetworkCredential("swadmin@meyer-mil.com", "meyer-mil.com");
                    _with2.Send(MailMsg);
                }
                catch (System.Exception ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    //Abandon variable
                    mSmtpClient = null;
                    foreach (Attachment aAttach in MailMsg.Attachments)
                    {
                        aAttach.Dispose();
                    }
                    MailMsg.Attachments.Dispose();
                    MailMsg.Dispose();
                    MailMsg = null;
                }
                return "Sent";
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}

