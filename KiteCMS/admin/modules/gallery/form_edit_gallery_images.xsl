<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="pageid" select="-1" />
<xsl:variable name="imagefolder"><xsl:value-of select="/website/gallery[@pageid=$pageid]/imagefolder"/></xsl:variable>
<xsl:template match="/">

  <form action="default.aspx" method="post" name="formedit">
    <input type="hidden" name="action" value="doeditgalleryimages"/>
    <input type="hidden" name="pageid" value="{//selectedpage}"/>
    <table border="0" width="650" cellpadding="4" cellspacing="0">
        <tr>
          <td width="151" align="center" style="border-bottom:dotted 1px #000"> </td>
          <td width="499" valign="top" style="border-bottom:dotted 1px #000">
            <label for="gallerydownloadtext" style="width:150px;display:block;float:left;">
              <xsl:value-of select="util:localText('gallerydownloadtext')"/>
            </label>&#160;
            <input type="text" name="gallerydownloadtext" id="gallerydownloadtext" class="alminput" size="40" maxlength="250" value="{/website/gallery[@pageid=$pageid]/gallerydownloadtext}"/>
          </td>
        </tr>
        <xsl:for-each select="/website/gallery[@pageid=$pageid]/folder">
			<tr>
				<td align="top" style="border-bottom:dotted 1px #000"><b><xsl:value-of select="folderpath"/></b></td>
				<td align="top" style="border-bottom:dotted 1px #000">
		            <label for="file{filename}_alt" style="width:150px;display:block;float:left;">
						<xsl:value-of select="util:localText('heading')"/>
					</label>&#160;
					<input type="text" name="folder_{folderid}" id="folder_{folderid}" class="alminput" size="25" maxlength="250" value="{header}"/><br/>
				</td>
			</tr>
		</xsl:for-each>
        <xsl:for-each select="/website/gallery[@pageid=$pageid]/image">
		<xsl:sort select="fileid"/>
        <tr>
          <td width="151" align="center" style="border-bottom:dotted 1px #000">
            <img src="/modules/gallery/images/thumbs/getImage.aspx?imageurl={folder}/{filename}&amp;maxwidth=75&amp;maxheight=75"/>
            <br/>
            <xsl:value-of select="filename"/>
          </td>
          <td width="499" valign="top" style="border-bottom:dotted 1px #000">
            <label for="file{filename}_alt" style="width:150px;display:block;float:left;">
              <xsl:value-of select="util:localText('alttag')"/>
            </label>&#160;
            <input type="text" name="filealt_{folder}:{filename}" id="filealt_{folder}:{filename}" class="alminput" size="25" maxlength="250" value="{alt}"/><br/>

            <label for="file{filename}_title" style="width:150px;display:block;float:left;">
              <xsl:value-of select="util:localText('gallerytitletag')"/>
            </label>&#160;
            <input type="text" name="filetitle_{folder}:{filename}" id="filetitle_{folder}:{filename}" class="alminput" size="40" maxlength="250" value="{title}"/><br/>
          </td>
        </tr>
      </xsl:for-each>

      <tr>
        <td></td>
        <td>
          <input type="submit" class="almknap" value="{util:localText('save')}" />&#160;
          <input type="button" class="almknap" value="{util:localText('cancel')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
        </td>
      </tr>
    </table>



  </form>
</xsl:template>

</xsl:stylesheet>