<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="debateid" select="-1" />
<xsl:param name="categoryid" select="-1" />
<xsl:param name="pageid" select="-1" />
<xsl:param name="dateShowNew" select="-1" />

<xsl:template match="/">
<script type="text/javascript">
function funcSubmit(){
	rateDebate.action.value='doactivedebate';
	rateDebate.submit();
}

function funcActivateRated(){
	rateDebate.action.value='modify';
	if (rateDebate.ratedbody.value.length==0){
		alert('<xsl:value-of select="util:localText('contentmissing')"/>');
		rateDebate.ratedbody.focus();
		return false;
	}
	rateDebate.submit();
}
</script>

<xsl:if test="$debateid!=''">
<xsl:for-each select="//item[@id=$debateid]">

	<form action="default.aspx?pageid={$pageid}" method="post" name="rateDebate" onsubmit="return funcSubmit();">
	<input type="hidden" name="action" value="doactivedebate"/>
	<input type="hidden" name="debateid" value="{$debateid}"/>
	<table border="0">
	<tr>
		<td valign="top" width="300" class="debate">
			<h3><xsl:value-of select="header"/>&#160;<xsl:call-template name="newdebate" /></h3>
	
			<xsl:value-of select="util:localText('created')"/>: <xsl:value-of select="@date"/><br/>
			<xsl:value-of select="util:localText('by')"/>: <xsl:value-of select="author"/><br/>
			<xsl:value-of select="util:localText('email')"/>: <xsl:value-of select="email"/><br/><br/>				
			
			<input type="button" onclick="location.href='default.aspx?action=activedebate&amp;pageid={$pageid}'" class="almknap" value="{util:localText('back')}"/><br/><br/>
	
			<xsl:value-of select="util:localText('originaltext')"/>:<br/>
			<xsl:value-of select="body"/><br/><br/>
			<xsl:if test="@active='0'">
				<input type="submit" value="Aktiver" class="almknap"/>
			</xsl:if>
			<br/><br/>
			<xsl:value-of select="util:localText('modifiedtext')"/>:<br/>
			<textarea cols="59" rows="10" name="ratedbody" class="alminput"><xsl:value-of select="ratedbody"/></textarea>
		</td>
	</tr>
	<tr>
		<td class="debate">
			<xsl:if test="@active='0'">
				<input type="button" value="{util:localText('activatemodified')}" onclick="funcActivateRated();" class="almknap"/>
			</xsl:if>
			<xsl:if test="@active='1'">
				<input type="button" value="{util:localText('modify')}" onclick="funcActivateRated();" class="almknap"/>
			</xsl:if>
			&#160;<input type="button" value="{util:localText('cancel')}" onclick="location.href='/default.aspx?pageid={$pageid}'" class="almknap"/>
		</td>
	</tr>
	
	</table>
	</form>
</xsl:for-each>
</xsl:if>					

</xsl:template>

<xsl:template name="newdebate">
	<xsl:if test="(substring-before(@date,'-') &gt; substring-before($dateShowNew,'-') and substring-after(substring-before(@date,' '),'-') = substring-after(substring-before($dateShowNew,' '),'-')) or (substring-before(substring-after(substring-before(@date,' '),'-'),'-')+12*substring-after(substring-after(substring-before(@date,' '),'-'),'-') &gt; substring-before(substring-after(substring-before($dateShowNew,' '),'-'),'-')+12*substring-after(substring-after(substring-before($dateShowNew,' '),'-'),'-'))">
    <span style='color:darkred'><sup><xsl:value-of select="util:localText('new')"/></sup></span>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>