<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

<xsl:template match="/">

<xsl:variable name="files">
	<xsl:for-each select="/website/folder/file"><xsl:value-of select="translate(@name,'\','')"/>,</xsl:for-each>
</xsl:variable>

<script type="text/javascript">
window.onload = function() { funcSelect('');funcFiles('<xsl:value-of select="$files"/>'); } 

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
function funcFiles(files){
	var arrFile = files.split(",");
	for(var i=document.getElementById('file').options.length; i!=-1; i--){
		document.getElementById('file').options[i] = null;
	}
	for(var i=0;i!=arrFile.length-1;i++){
		document.getElementById('file').options.add(new Option(arrFile[i],arrFile[i]));
	}

}
function funcSubmit(){
	if(confirm('<xsl:value-of select="util:localText('confirmdeletefile')"/>'))
		return true;
	else
		return false;
}
</script>


	<table border="0" width="450">
	<tr>
		<td>
			<form method="post" action="deletefile.aspx?action=dodeletefile&amp;pageid={//selectedpage}" name="upload" enctype="multipart/form-data" onsubmit="return funcSubmit();">
	
			<div class="FolderContainer scrollbar">
				<strong><xsl:value-of select="util:localText('choosedeletefolder')"/></strong><br/><br/>
		
				<xsl:for-each select="/website/folder">
				<div>
					<a id="folderdiv{translate(@uploadrootpath,'\',':')}" style="border: solid 2px green;" href="javascript:void(0);" onfocus="this.blur()" onclick="funcSelect('{translate(@uploadrootpath,'\',':')}');funcFiles('{$files}');"><img src="/admin/images/folder.gif" id="image{translate(@uploadrootpath,'\',':')}" alt="" border="0"/>&#160;<xsl:value-of select="@name"/></a>
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
        <select name="file" id="file" size="10" style="width:200px;" multiple="multiple"><option/></select><br/>
		   <input type="submit" value="{util:localText('delete')}" class="almknap"/>&#160;
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
		<a id="folderdiv{translate(@uploadrootpath,'\',':')}" onfocus="this.blur()" href="javascript:void(0);" onclick="funcSelect('{translate(@uploadrootpath,'\',':')}');funcFiles('{$files}');"><img src="/admin/images/folder.gif" id="image{translate(@uploadrootpath,'\',':')}" alt="" border="0"/>&#160;<xsl:value-of select="@name"/></a><br/>
	</div>
</xsl:template>

</xsl:stylesheet>