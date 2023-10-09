<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="user" select="-1" />

<xsl:template match="/">
<script type="text/javascript">
	function funcSubmit(){
	if(document.addUser.username.value==""){
		alert("<xsl:value-of select="util:localText('loginnotempty')"/>");
		document.addUser.username.focus();
		return false;
		}
	if(document.addUser.password.value!=document.addUser.password2.value){
		alert("<xsl:value-of select="util:localText('passwordnotequal')"/>");
		document.addUser.password2.focus();
		return false;
		}
	if(document.addUser.password.value.length == 0){
		alert("<xsl:value-of select="util:localText('passwordnotempty')"/>");
		document.addUser.password.focus();
		return false;
		}
	return true
	}
</script>
<xsl:text disable-output-escaping="yes">
<![CDATA[
	<script type="text/javascript">
  <!-- //
  function funcClick(intId){
		oCheckbox = eval("document.addUser.id"+ intId)
		for (var counter=0; counter != document.addUser.length; counter++){
			if (document.addUser.elements[counter].type=='checkbox'&& document.addUser.elements[counter].name.indexOf('id') == 0)
				if (document.addUser.elements[counter].name != 'id0'){
					if (intId == 0){
						if (oCheckbox.checked == true){
							document.addUser.elements[counter].checked = false
							document.addUser.elements[counter].disabled = true
						} else {
							document.addUser.elements[counter].disabled = false
						}
					} else if(oCheckbox.name.substring(4,6)=="00"){
						if (document.addUser.elements[counter].name.substring(0,4) == oCheckbox.name.substring(0,4))
						if(document.addUser.elements[counter].name.substring(4,6)!="00"){
							if (oCheckbox.checked == true){
								document.addUser.elements[counter].checked = false
								document.addUser.elements[counter].disabled = true
							} else {
								document.addUser.elements[counter].disabled = false
							}
						}
					}
				}
		}
	}
  //-->
</script>
]]>
</xsl:text>	

	<form method="post" action="adduser.aspx" name="addUser" onsubmit="return funcSubmit();">
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="action" value="add"/>
	
	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('loginname')"/></td>
		<td><input type="text" class="alminput" name="username"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('fullname')"/></td>
		<td><input type="text" class="alminput" name="fullname"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('password')"/></td>
		<td><input type="password" class="alminput" name="password" onfocus="this.value=''"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('repeatpassword')"/>&#160;</td>
		<td><input type="password" class="alminput" name="password2" onfocus="this.value=''"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('active')"/></td>
		<td>
		<input type="checkbox" name="active" value="1" checked="checked"/>
		</td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('canchangepassword')"/></td>
		<td>
		<input type="checkbox" name="changepassword" value="1"/>
		</td>
	</tr>
	<tr>
		<td></td>
		<td><input type="submit" class="almknap" value="{util:localText('save')}"/>
		&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</td>
	</tr>
	</table>
<br/>
<table cellpadding="5" cellspacing="0" border="0">
<tr>
<td valign="top" width="200">
<content/>
</td>
<td valign="top" width="5" style="border-left:dotted 1px #000">&#160;</td>
<td valign="top">
      <table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><b><u><xsl:value-of select="util:localText('rights')"/></u></b></td>
	</tr>	
	<xsl:for-each select="//module[@id=0]">
	<tr>
		<td width="200">
			<xsl:element name="input">
				<xsl:attribute name="type">checkbox</xsl:attribute>
				<xsl:attribute name="name">id<xsl:value-of select="@id"/></xsl:attribute>
				<xsl:attribute name="value">1</xsl:attribute>
				<xsl:attribute name="onclick">funcClick(0)</xsl:attribute>
				<xsl:if test="//user[@id=$user and @active='1']/access=@id">
          <xsl:attribute name="checked">checked</xsl:attribute>
				</xsl:if>
			</xsl:element>
			<b><xsl:value-of select="util:localText(./@name)"/></b><br/>
		</td>
	</tr>
	</xsl:for-each>
	<xsl:for-each select="//module/module[@id &lt;2000]">
	<tr>
		<td width="200">
			&#160;&#160;
			<xsl:element name="input">
				<xsl:attribute name="type">checkbox</xsl:attribute>
				<xsl:attribute name="name">id<xsl:value-of select="@id"/></xsl:attribute>
				<xsl:attribute name="value">1</xsl:attribute>
				<xsl:attribute name="onclick">funcClick(<xsl:value-of select="@id"/>)</xsl:attribute>
				<xsl:if test="//user[@id=$user and @active='1']/access=@id">
          <xsl:attribute name="checked">checked</xsl:attribute>
        </xsl:if>
			</xsl:element>
			<b><xsl:value-of select="@name"/></b><br/>
			<xsl:for-each select="method">
				&#160;&#160;&#160;&#160;
				<xsl:element name="input">
					<xsl:attribute name="type">checkbox</xsl:attribute>
					<xsl:attribute name="name">id<xsl:value-of select="@id"/></xsl:attribute>
					<xsl:attribute name="value">1</xsl:attribute>
					<xsl:attribute name="onclick">funcClick(<xsl:value-of select="@id"/>)</xsl:attribute>
					<xsl:if test="//user[@id=$user and @active='1']/access=@id">
            <xsl:attribute name="checked">checked</xsl:attribute>
          </xsl:if>
				</xsl:element>
				<xsl:value-of select="util:localText(./@name)"/><br/>
			</xsl:for-each>	
		</td>
	</tr>
	</xsl:for-each>
	</table>
</td>
<td valign="top">
	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td height="33" valign="bottom">&#160;<br/><br/><b><xsl:value-of select="util:localText('templaterights')"/></b></td>
	</tr>	

	<xsl:for-each select="//module/module[@id &gt;1999]">
	<tr>
		<td>
			&#160;&#160;
			<xsl:element name="input">
				<xsl:attribute name="type">checkbox</xsl:attribute>
				<xsl:attribute name="name">id<xsl:value-of select="@id"/></xsl:attribute>
				<xsl:attribute name="value">1</xsl:attribute>
				<xsl:attribute name="onclick">funcClick(<xsl:value-of select="@id"/>)</xsl:attribute>
				<xsl:if test="//user[@id=$user and @active='1']/access=@id">
          <xsl:attribute name="checked">checked</xsl:attribute>
        </xsl:if>
			</xsl:element>
			<b><xsl:value-of select="util:localText(./@name)"/></b><br/>
			<xsl:for-each select="method">
				&#160;&#160;&#160;&#160;
				<xsl:element name="input">
					<xsl:attribute name="type">checkbox</xsl:attribute>
					<xsl:attribute name="name">id<xsl:value-of select="@id"/></xsl:attribute>
					<xsl:attribute name="value">1</xsl:attribute>
					<xsl:attribute name="onclick">funcClick(<xsl:value-of select="@id"/>)</xsl:attribute>
					<xsl:if test="//user[@id=$user and @active='1']/access=@id">
            <xsl:attribute name="checked">checked</xsl:attribute>
          </xsl:if>
				</xsl:element>
				<xsl:value-of select="util:localText(./@name)"/><br/>
			</xsl:for-each>	
		</td>
	</tr>
	</xsl:for-each>
	</table>
</td>

</tr>
	
</table>
</form>

</xsl:template>
</xsl:stylesheet>
