<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="selectedshopitem" select="-1" />

<xsl:template match="/">

	<script language="javascript" type="text/JavaScript">
	var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
	var gl_fieldname;
	function funcSubmit(frm)
	{
	<xsl:for-each select="//field[translate(@required,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')='TRUE']">
		if (frm.shopitem<xsl:value-of select="@id"/>.value == ''){
			alert('<xsl:value-of select="util:localText('fieldmissing')"/><xsl:value-of select="@label"/>');
			frm.shopitem<xsl:value-of select="@id"/>.focus();
			return false;}
	</xsl:for-each>
	return true;
	}
	
	function funcMedia(fieldname){
		gl_fieldname = fieldname;
		if (isIE){
			var arr = showModalDialog("/admin/editor/includes/image.html", "", "font-family:Verdana; font-size:10; dialogWidth:700px; dialogHeight:550px;status:no;help:no" );
			if (arr != null){
				if (arr["src"] != '') {
					document.getElementById(fieldname).value = arr["src"];
				}
			}
		} else {
			winModalWindow = window.open ("/admin/editor/includes/image.html","ModalChild","dependent=yes,width=700,height=550")
			winModalWindow.focus();
		}
	}

	function funcImageSelected(img,alt,title){
		document.getElementById(gl_fieldname).value= img;
		parent.window.close();
	}
	</script>

	<table cellpadding="2" cellspacing="0" border="0">
	<form name="shopitem" id="shopitem" onsubmit="return funcSubmit(this);" method="post" action="default.aspx">
	<input type="hidden" name="action" value="doaddshopitem"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<xsl:for-each select="/shop/productstore/field">
		<xsl:variable name="name"><xsl:value-of select="@id"/></xsl:variable>
		<tr><td valign="top"><label for="shopitem{$name}"><xsl:value-of select="@label"/></label></td><td>
		<xsl:choose>
			<xsl:when test="@id='category'">
				<select id="shopitem{$name}" name="shopitem{$name}" class="alminput">
					<option value=""><xsl:value-of select="util:localText('choosecategory')"/></option>
					<xsl:for-each select="//category">
						<xsl:element name="option">
							<xsl:if test="//shopitem[@id=$selectedshopitem]/item[@id=$name]=@value">
								<xsl:attribute name="selected"/>
							</xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@id"/></xsl:attribute>
							<xsl:if test="ancestor::category">&#160;-&#160;</xsl:if>
							<xsl:value-of select="title"/>
						</xsl:element>
					</xsl:for-each>
				</select>
			</xsl:when>
			<xsl:when test="@type='text'">
				<input id="shopitem{$name}" name="shopitem{$name}" size="{@size}" class="alminput" value="{//shopitem[@id=$selectedshopitem]/item[@id=$name]}"/>
			</xsl:when>
			<xsl:when test="@type='largetext'">
				<textarea id="shopitem{$name}" name="shopitem{$name}" class="alminput" cols="{@cols}" rows="{@rows}"><xsl:value-of select="//shopitem[@id=$selectedshopitem]/item[@id=$name]"/></textarea>
			</xsl:when>
			<xsl:when test="@type='picture'">
				<input id="shopitem{$name}" name="shopitem{$name}" size="{@size}" class="alminput" value="{//shopitem[@id=$selectedshopitem]/item[@id=$name]}"/>
				<a href="javascript:void(0);" onclick="funcMedia('shopitem{$name}');"><img src="/admin/editor/images/image.gif" align="middle" border="0"/></a>
			</xsl:when>
			<xsl:when test="@type='select'">
				<select id="shopitem{$name}" name="shopitem{$name}" class="alminput">
					<xsl:for-each select="option">
						<xsl:element name="option">
							<xsl:if test="//shopitem[@id=$selectedshopitem]/item[@id=$name]=@value">
								<xsl:attribute name="selected"/>
							</xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
							<xsl:value-of select="@label"/>
						</xsl:element>
					</xsl:for-each>
				</select>
			</xsl:when>
			<xsl:when test="@type='checkbox'">
				<xsl:element name="input">
					<xsl:attribute name="type">checkbox</xsl:attribute>
					<xsl:attribute name="name">shopitem<xsl:value-of select="$name"/></xsl:attribute>
					<xsl:attribute name="id">shopitem<xsl:value-of select="$name"/></xsl:attribute>
					<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
					<xsl:if test="//shopitem[@id=$selectedshopitem]/item[@id=$name]!=''">
						<xsl:attribute name="checked"/>
					</xsl:if>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
		</td></tr>
	</xsl:for-each>
	<tr><td></td><td><input value="{util:localText('save')}" type="submit" class="almknap"/>&#160;<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/></td></tr>
	</form>
	</table>
</xsl:template>
</xsl:stylesheet>