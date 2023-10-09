<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="user" select="-1" />

<xsl:template match="/">

<xsl:variable name="menuIDs">
	<xsl:for-each select="//user[@id=$user]/menuid">|<xsl:value-of select="."/>|</xsl:for-each>
</xsl:variable>


<script type="text/javascript">
  window.onload = function() { funcStart('<xsl:value-of select="$menuIDs"/>'); }
  var passwordChanged = false;
  function funcSubmit(){
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
	if (passwordChanged == false)	
		document.addUser.password.value ="";
	
	return true;
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

	function funcStart(arrMenuIDs){
		for (var counter=0; counter != document.addUser.length; counter++){
			if (document.addUser.elements[counter].type=='checkbox'&& document.addUser.elements[counter].name.indexOf('id') == 0){
				if ( document.addUser.id0.checked==true)
					if (document.addUser.elements[counter].name != 'id0'){
						document.addUser.elements[counter].checked = false
						document.addUser.elements[counter].disabled = true
					}
				if (!(document.addUser.elements[counter].name == 'id0' || document.addUser.elements[counter].name.substring(4,6)=='00')){
					if (eval("document.addUser."+document.addUser.elements[counter].name.substring(0,4) + "00" +".checked") == true){
						document.addUser.elements[counter].checked = false
						document.addUser.elements[counter].disabled = true
					}
				}
			}
			if (document.addUser.elements[counter].type=='checkbox'&& document.addUser.elements[counter].name.indexOf('menuid') == 0){
				var arrId = document.addUser.elements[counter].id.split("|")
				for (var counter3=0; counter3 != arrId.length; counter3++)
					if ((arrId[counter3]!='' && (arrMenuIDs.indexOf("|"+arrId[counter3]+"|")>-1)) || (arrMenuIDs.indexOf("|"+document.addUser.elements[counter].name.replace('menuid','')+"|")>-1)||(arrMenuIDs.indexOf("|0|")>-1)){
						if (arrMenuIDs.indexOf("|"+document.addUser.elements[counter].name.replace('menuid','')+"|")==-1 &&document.addUser.elements[counter].name!='menuid0'){
							document.addUser.elements[counter].checked = false;
							document.addUser.elements[counter].disabled = true;
						} else {
							document.addUser.elements[counter].checked = true;
							document.addUser.elements[counter].disabled = false;
						}
					}
			}
		}
	}
  // -->
</script>
]]>
</xsl:text>	

	<form method="post" action="edituser.aspx" name="addUser" onsubmit="return funcSubmit();">
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="action" value="edit"/>
	<input type="hidden" name="userid" value="{$user}"/>
	
	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('loginname')"/></td>
		<td><b><xsl:value-of select="//user[@id=$user]/username"/></b></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('fullname')"/></td>
		<td><input type="text" class="alminput" name="fullname" value="{//user[@id=$user]/fullname}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('password')"/></td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">password</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
				<xsl:attribute name="name">password</xsl:attribute>
				<xsl:attribute name="value">password</xsl:attribute>
				<xsl:attribute name="onfocus">this.value='';passwordChanged = true;</xsl:attribute>
				<xsl:attribute name="onblur">if(this.value=='') {this.value='password';passwordChanged = false;}</xsl:attribute>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('repeatpassword')"/>&#160;</td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">password</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
				<xsl:attribute name="name">password2</xsl:attribute>
				<xsl:attribute name="value">password</xsl:attribute>
				<xsl:attribute name="onfocus">this.value='';passwordChanged = true;</xsl:attribute>
				<xsl:attribute name="onblur">if(this.value=='') {this.value='password';passwordChanged = false;}</xsl:attribute>
			</xsl:element>
		</td>	
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('active')"/></td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">checkbox</xsl:attribute>
				<xsl:attribute name="name">active</xsl:attribute>
				<xsl:attribute name="value">1</xsl:attribute>
				<xsl:if test="//user[@id=$user and @active='1']">
          <xsl:attribute name="checked">checked</xsl:attribute>
        </xsl:if>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('canchangepassword')"/></td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">checkbox</xsl:attribute>
				<xsl:attribute name="name">changepassword</xsl:attribute>
				<xsl:attribute name="value">1</xsl:attribute>
				<xsl:if test="//user[@id=$user and @changepassword='1']">
          <xsl:attribute name="checked">checked</xsl:attribute>
        </xsl:if>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('resetcount')" disable-output-escaping="yes"/> = <xsl:value-of select="//user[@id=$user]/@failedlogins"/>)</td>
		<td valign="top"><input type="checkbox" value="1" name="resetFailedLogins"/></td>
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
