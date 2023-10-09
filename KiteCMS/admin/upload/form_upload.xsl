<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

<xsl:param name="strAllowedFilesList" select="-1" />
<xsl:param name="intMaxFileSizeKB" select="-1" />
  <xsl:param name="userCanOverwrite" select="-1" />
  <xsl:param name="embedded" select="-1" />

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
<xsl:variable name="width"><xsl:choose><xsl:when test="$embedded='true'">300</xsl:when><xsl:otherwise>450</xsl:otherwise></xsl:choose></xsl:variable>
  
<div id="divForm" style="display:block">
	<form method="post" action="upload.aspx?action=doupload&amp;pageid={//selectedpage}" name="upload" enctype="multipart/form-data" onsubmit="document.getElementById('divForm').style.display='none';document.getElementById('divWait').style.display='block'">
		<table border="0" width="{$width}">
		<tr>
			<td>
			<p>
			<xsl:value-of select="util:localText('uploadtypes')"/><br/> "<xsl:value-of select="$strAllowedFilesList"/>"<br/>
			<xsl:value-of select="util:localText('maxuploadsize')"/>&#160;<xsl:value-of select="$intMaxFileSizeKB"/> kb<br/>
			</p>
	
			<hr class="dottedHr"/>
	
            <div class="FolderContainer scrollbar Embedded_{$embedded}">
                <strong><xsl:value-of select="util:localText('chooseuploadfolder')"/></strong><br/><br/>

                <xsl:for-each select="/website/folder">
                    <div>
                    <a id="folderdiv{translate(@uploadrootpath,'\',':')}" style="border: solid 2px green;" href="javascript:void(0);" onfocus="this.blur()" onclick="funcSelect('{translate(@uploadrootpath,'\',':')}');"><img src="/admin/images/folder.gif" id="image{translate(@uploadrootpath,'\',':')}" alt="" border="0"/>&#160;<xsl:value-of select="@name"/></a>
                    </div>
                    <div>
                        <xsl:for-each select="descendant::folder">
                            <xsl:call-template name="folder"/>
                        </xsl:for-each>
                    &#160;</div>
                </xsl:for-each>
            </div>
	
	
			<hr class="dottedHr"/>

	      <xsl:if test="$embedded='true'">
	        <input type="hidden" name="simple" value="true"/>        
	      </xsl:if>
		   <input type="hidden" name="folder" id="folder" value=""/>
		   <input type="file" name="file" class="alminput" size="30"/><br/>
		   <input type="file" name="file" class="alminput" size="30"/><br/>
		   <input type="file" name="file" class="alminput" size="30"/><br/>
		   <input type="file" name="file" class="alminput" size="30"/><br/>
           <input type="file" name="file" class="alminput" size="30"/><br/>
            <xsl:if test="$embedded!='true'">
                <input type="file" name="file" class="alminput" size="30"/><br/>
                <input type="file" name="file" class="alminput" size="30"/><br/>
                <input type="file" name="file" class="alminput" size="30"/><br/>
                <input type="file" name="file" class="alminput" size="30"/><br/>
                <input type="file" name="file" class="alminput" size="30"/><br/>
            </xsl:if>
            <xsl:element name="input">
		   	<xsl:attribute name="type">checkbox</xsl:attribute>
		   	<xsl:attribute name="name">overwrite</xsl:attribute>
		   	<xsl:attribute name="value">true</xsl:attribute>
			<xsl:if test="$userCanOverwrite!='true'">
   			   	<xsl:attribute name="disabled">true</xsl:attribute>
			</xsl:if>
		   </xsl:element>
		   <xsl:value-of select="util:localText('overwriteexisting')"/><br/><br/>
		   <input type="submit" value="{util:localText('upload')}" class="almknap"/>&#160;
	      <xsl:if test="not($embedded='true')">
	        <input type="button" value="{util:localText('cancel')}" class="almknap" onclick="location.href='/default.aspx?pageid={//selectedpage}'"/>
	      </xsl:if>
	      <xsl:if test="$embedded='true'">
	        <input type="reset" value="{util:localText('cancel')}" class="almknap"/>
	      </xsl:if>
			</td>
		</tr>
		</table>
  </form>
	</div>
<div id="divWait" style="display:none">
  <span style='color:red'><xsl:value-of select="util:localText('uploadpleasewait')"/></span>
</div>

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