<%@ Control Language="c#" AutoEventWireup="false" Codebehind="IEeditor.ascx.cs" Inherits="KiteCMS.Admin.IEeditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

	<script language="javascript">
		// Defining global variables
		var chooseText = "<% Response.Write(KiteCMS.Functions.localText("chooseText")); %>";
		var hasjavascriptText = "<% Response.Write(KiteCMS.Functions.localText("hasjavascript")); %>";
		
		function MENU_FILE_NOTSAVE_onclick() {
		
		if(confirm("<% Response.Write(KiteCMS.Functions.localText("leaveWithoutSaving")); %>"))
			document.location.href=('/admin/default.aspx?pageid=<% Response.Write(pageId); %>');
		}
	</script>

<asp:Literal Runat="server" id="litJavascript"></asp:Literal>

<div class="tbToolbar" id="warning" style="margin-top:-20px;visibility:visible;width:700px;">If you see this text, you are not able to use the editor. See the <a href="http://www.KiteCMS.com/help" target="blank">online help</a> for help on solving this problem</div>
<script type="text/javascript">document.getElementById('warning').style.visibility='hidden';</script>
<table class="tbToolbar" style="visibility:hidden" id="tbToolbar" cellspacing="0" cellpadding="0" border=0>
<tr>
<td>
	<div id="divSave" class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("save"));%>" onclick="return MENU_FILE_SAVE_onclick();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/save.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div id="divCancel" class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("cancel"));%>" onclick="return MENU_FILE_NOTSAVE_onclick();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/nosave.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td width=1 class="tbSeparator"><img src=/admin/images/space.gif width=1 height=1/></td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("bold"));%>" onclick="document.execCommand('Bold',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/bold.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("italic"));%>" onclick="document.execCommand('Italic',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/Italic.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("underline"));%>" onclick="document.execCommand('Underline',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/under.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td width=1 class="tbSeparator"><img src="/admin/images/space.gif" width="1" height="1"/></td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("copy"));%>" onclick="document.execCommand('copy',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/copy.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("cut"));%>" onclick="document.execCommand('cut',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/cut.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("paste"));%>" onclick="focusCurrentContentholder();document.execCommand('paste',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/paste.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("paste_word"));%>" onclick="focusCurrentContentholder();funcPasteWord();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/pasteWord.gif" />
	</div>
</td>
<td width=1 class="tbSeparator"><img src=/admin/images/space.gif width=1 height=1/></td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("align_left"));%>" onclick="document.execCommand('justifyLeft',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/left.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("align_center"));%>" onclick="document.execCommand('justifycenter',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/center.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("align_right"));%>" onclick="document.execCommand('justifyright',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/right.gif" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td width=1 class="tbSeparator"><img src=/admin/images/space.gif width=1 height=1/></td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("insert_link"));%>" onclick="funcLink();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/link.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("insert_anchor"));%>" onclick="funcAnchor();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/anchor.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("insert_media"));%>" onclick="funcMedia();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/media.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("remove_link"));%>" onclick="funcUnlink();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/unlink.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td width=1 class="tbSeparator"><img src=/admin/images/space.gif width=1 height=1/></td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("bullist"));%>" onclick="focusCurrentContentholder();document.execCommand('insertunorderedlist',false,null);" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/bullist.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("font_color"));%>" onclick="funcColor();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/fgcolor.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("insert_picture"));%>" onclick="funcImage();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/image.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td class=tbButton>
	<select id="selectClass" onchange="funcSetStyle(this[this.selectedIndex].value);this.selectedIndex=0" class="alminput">
	<option selected><% Response.Write(KiteCMS.Functions.localText("text_class"));%></option>
	<option value="P"><% Response.Write(KiteCMS.Functions.localText("normal"));%></option>
	<option value="H1"><% Response.Write(KiteCMS.Functions.localText("heading"));%> 1</option>
	<option value="H2"><% Response.Write(KiteCMS.Functions.localText("heading"));%> 2</option>
	<option value="H3"><% Response.Write(KiteCMS.Functions.localText("heading"));%> 3</option>
	<asp:Literal ID=options Runat=server/>
	</select> 
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("insert_table"));%>" onclick="InsertTable();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/instable.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("insert_tablerow"));%>" onclick="InsertTableRow();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/insrow.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td width=1 class="tbSeparator"><img src=/admin/images/space.gif width=1 height=1/></td>
<td>
	<div id="divText" class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("text_html"));%>" onclick="funcToggleMode();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/details.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
<td>
	<div class="tbButton" TITLE="<% Response.Write(KiteCMS.Functions.localText("visible_border"));%>" onclick="funcToggleBorder();" onmouseover="button_over(this);" onmouseout="button_out(this);">
		<img class="tbIcon" src="/admin/editor/images/borders.GIF" WIDTH="23" HEIGHT="22"/>
	</div>
</td>
</tr>
</table>

	<asp:label id="editor" Runat="server"></asp:label>
