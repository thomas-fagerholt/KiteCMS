<%@ Page language="c#" Codebehind="mailform.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.mailform" %>
<asp:Literal Runat="server" id="lbContent"></asp:Literal>
<!--

'####################################
'##
'##	This mailform is developted by and for KiteCMS
'##	
'##	The mailform shall be submitted to this ASPX-file.
'##	The mailform shall have method=post and the fields below shall be part of 
'##	the form (either as hidden or visible-fields). If not all fields are defined
'##	with content, there will not be send any mail.
'##	
'## 	Required fields:
'##		mailTo: The receiver of the mails (Either the whole emailadress or the part 
'##		before @domain.tld and then the domain will be taken from the url of the homepage)
'##		mailSubject: Subject of the email
'##		ReturnURL: The page to display after sending the mail
'##
'##		Possible other fields:
'##		mailHeaderText: Text in the mail before the field values 
'##		mailFooterText: Text in the mail after the field values 
'## 	fieldsList: Fields from the form, which values should be in the mail
'##
'##		The ReturnURL is called with a querystringparameter mailstatus=ok
'##		A javascript confirm popup can be made with this code:
'##
'##	<script type='text/javascript'>
'##	var strLocation = '' + this.location;
'##	if (strLocation.indexOf("mailstatus=ok")>-1)
'##		alert('Thank you for your feedback.');
'##	</script>
'##
'####################################

-->
