<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.ModulePage" ValidateRequest="False" %>
<asp:Literal Runat="server" id="lbContent"></asp:Literal>
<form id="theform" visible="false" runat="server">
<asp:PlaceHolder id="phContent" runat="server" />
</form>
<asp:Literal Runat="server" id="lbAfterForm"/>
