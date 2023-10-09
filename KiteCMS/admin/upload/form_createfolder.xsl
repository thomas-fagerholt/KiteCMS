<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

<xsl:template match="/">

<script type="text/javascript">
window.onload = function() { funcSelect(document.getElementById('folder').value); } 

function funcSelect(id){
	document.getElementById('folder').value = id;
	for (var i = 0; i != document.getElementsByTagName("a").length; i++) {
		if (document.getElementsByTagName("a")[i].style.border != null)
		{
			document.getElementsByTagName("a")[i].style.border = "0px";
			oldid = document.getElementsByTagName("a")[i].id;
			if (oldid!="")
				document.getElementById(oldid.replace('folderdiv','image')).src = '/admin/images/folder.gif';
		}
	}
	document.getElementById("folderdiv"+ id).style.border = "solid 2px green";
	document.getElementById("image"+ id).src = "/admin/images/folder_open.gif";
}
</script>

	<table border="0" width="450">
	<tr>
		<td>
		</td>
	</tr>
	<tr>
		<td>
			<form method="post" action="createfolder.aspx?action=docreatefolder&amp;pageid={//selectedpage}" name="upload" enctype="multipart/form-data" onsubmit="return funcSubmit();">
			<div class="FolderContainer scrollbar">
				<strong><xsl:value-of select="util:localText('chooseparentfolder')"/></strong><br/><br/>
		
				<xsl:for-each select="/website/folder">
				<div>
					<a id="folderdiv{translate(@uploadrootpath,'\',':')}" style="border: solid 2px green;" href="javascript:void(0);" onfocus="this.blur()" onclick="funcSelect('{translate(@uploadrootpath,'\',':')}');"><img src="/admin/images/folder.gif" id="image{translate(@uploadrootpath,'\',':')}" alt="" border="0"/>&#160;<xsl:value-of select="@name"/></a>
				</div>
					<div>
			      <xsl:for-each select="descendant::folder">
	            <xsl:call-template name="folder"/>
	          </xsl:for-each>
					</div>
				</xsl:for-each>
			</div>
	
			<hr class="dottedHr"/>

			   <input type="hidden" name="folder" id="folder" value=""/><br/>
			   <xsl:value-of select="util:localText('name')"/>:<br/>
			   <input type="text" name="newfolder" class="alminput" size="30" value=""/><br/>&#160;<br/>
			   <input type="submit" value="{util:localText('create')}" class="almknap"/>&#160;
				<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
			</form>

		</td>
	</tr>
	</table>

</xsl:template>

  <xsl:template name="folder">
	<xsl:variable name="files">
		<xsl:for-each select="file"><xsl:value-of select="translate(@name,'\','')"/>,</xsl:for-each>
	</xsl:variable>
	<div style="padding-left:{10*count(ancestor::folder)}px">
		<a id="folderdiv{translate(@uploadrootpath,'\',':')}" href="javascript:void(0);" onfocus="this.blur()" onclick="funcSelect('{translate(@uploadrootpath,'\',':')}');"><img src="/admin/images/folder.gif" id="image{translate(@uploadrootpath,'\',':')}" alt="" border="0"/>&#160;<xsl:value-of select="@name"/></a><br/>
	</div>
</xsl:template>
</xsl:stylesheet>