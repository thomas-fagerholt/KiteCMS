<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="canDelete" select="false"/>
<xsl:variable name="selectedevent"><xsl:value-of select="//selectedevent"/></xsl:variable>

<xsl:template match="/">

	<script language="javascript" type="text/JavaScript">
	function funcsubmit()
	{
		var dateReg = '^([0-9]{4})\-((0?[0-9]|1[0-2]))\-([0-2]?[0-9]|3[0-1])$';
		var regex = new RegExp(dateReg);

		if(!regex.test(document.getElementById('fielddate').value)){
			alert('<xsl:value-of select="util:localText('mustformatdateymd')"/>');
			document.getElementById('fielddate').focus();
			return false;
		}
	<xsl:for-each select="/website/calendar/field[translate(@required,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')='TRUE' and @type!='editor']">
		if (document.getElementById('field<xsl:value-of select="@id"/>').value == ''){
			alert('<xsl:value-of select="util:localText('fieldmissing')"/><xsl:value-of select="@label"/>');
			document.getElementById('field<xsl:value-of select="@id"/>').focus();
			return false;}
	</xsl:for-each>
	<xsl:for-each select="/website/calendar/field[translate(@required,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')='TRUE' and @type='editor']">
	    if (document.all) {
	      if(document.getElementById('tbContent_' + allEditors[0]).innerHTML==''){
	        alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
	        document.getElementById('tbContent_' + allEditors[0]).focus();
	        return false;
		    }
		  } else {
		    if(document.getElementById('tbContent_' + allEditors[0]).contentWindow.document.body.innerHTML==''){
		      alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
				  return false;
		    }
		  }
	</xsl:for-each>
	<xsl:choose>
		<xsl:when test="count(/website/calendar/field[@type='editor'])>0">
		  MENU_FILE_SAVE_onclick();
			return false;
		</xsl:when>
		<xsl:otherwise>
			return true;
		</xsl:otherwise>
	</xsl:choose>
	}
	</script>

	<table cellpadding="2" cellspacing="0" border="0">
	<xsl:text disable-output-escaping="yes">
	<![CDATA[
	  <form action="default.aspx" target="_self" method="post" name="tempForm" onsubmit="return funcsubmit();">
	]]>
	</xsl:text>	
	<input type="hidden" name="action" value="doeditcalendar"/>
	<input type="hidden" name="eventid" value="{$selectedevent}"/>
	<xsl:for-each select="/website/calendar/field">
		<xsl:variable name="name"><xsl:value-of select="@id"/></xsl:variable>
		<tr><td valign="top"><label for="field{$name}"><xsl:value-of select="@label"/></label></td><td>
		<xsl:choose>
			<xsl:when test="@type='editor'">
				<Editor id="field{$name}" configfile="/admin/data/editornosave.xml" noformtag="true" hidden="false" height="300px" />
			</xsl:when>
			<xsl:when test="@type='text'">
				<input id="field{$name}" name="field{$name}" size="{@size}" class="alminput" value="{//event[@id=$selectedevent]/item[@id=$name]}"/>
			</xsl:when>
			<xsl:when test="@type='largetext'">
				<textarea id="field{$name}" name="field{$name}" class="alminput" cols="{@cols}" rows="{@rows}"><xsl:value-of select="//event[@id=$selectedevent]/item[@id=$name]"/></textarea>
			</xsl:when>
			<xsl:when test="@type='select'">
				<select id="field{$name}" name="field{$name}" class="alminput">
					<xsl:for-each select="option">
						<xsl:element name="option">
							<xsl:if test="//event[@id=$selectedevent]/item[@id=$name]=@value">
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
					<xsl:attribute name="name">field<xsl:value-of select="$name"/></xsl:attribute>
					<xsl:attribute name="id">field<xsl:value-of select="$name"/></xsl:attribute>
					<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
					<xsl:if test="//event[@id=$selectedevent]/item[@id=$name]!=''">
						<xsl:attribute name="checked"/>
					</xsl:if>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
		</td></tr>
	</xsl:for-each>
	<tr>
		<td></td>
		<td>
			<input value="{util:localText('save')}" type="submit" class="almknap"/>&#160;
			<xsl:if test="$canDelete">
				<xsl:element name="input">
				<xsl:attribute name="type">button</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="util:localText('delete')"/></xsl:attribute>
				<xsl:attribute name="class">almknap</xsl:attribute>
				<xsl:attribute name="onclick">if(confirm('<xsl:value-of select="util:localText('oktodelete')"/>')) {document.location='default.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;eventid=<xsl:value-of select="$selectedevent"/>&amp;action=dodeletecalendar';}</xsl:attribute>
				</xsl:element>&#160;
			</xsl:if>
			<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/>
		</td>
	</tr>
	</table>
</xsl:template>
</xsl:stylesheet>