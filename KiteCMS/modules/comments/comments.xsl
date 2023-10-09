<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:util="urn:contentFunctions" exclude-result-prefixes="util">
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="pageid" select="-1" />
  <xsl:param name="rootpageid" select="-1" />
  <xsl:param name="action" select="-1" />

<xsl:template match="/">
  <hr class="CommentHR"/>
  <xsl:choose>
    <xsl:when test="util:GetQuerystring('keyword')!=''">
    <!-- keyword in querystring. A list af pages i generated and the comments shall not be shown -->  
    </xsl:when>
    <xsl:when test="$action='showcomment'">
      <xsl:for-each select="//commentItem[@pageid=$pageid and @active=1]">
        <xsl:sort select="@created"/>
        <div class="Comment">
          <h2><xsl:value-of select="item[@type='header']"/> <span class="CommentDate">by <xsl:value-of select="item[@type='author']"/>&#160;<xsl:value-of select="@created"/></span></h2>
          <div class="content"><xsl:value-of select="item[@type='comment']" disable-output-escaping="yes"/></div>
          <hr class="CommentHR"/>
        </div>
      </xsl:for-each>
      <div style="float:right">
        <a href="/default.aspx?pageid={//selectedpage}&amp;action=addcomment">Add comment</a>
      </div>
    </xsl:when>
    <xsl:when test="$action='commentadded'">
      <p class="CommentThankyoutext"><xsl:value-of select="//comments[@rootpageid=$rootpageid]/thankyoutext"/></p>
      <p><a href="/default.aspx?pageid={//selectedpage}&amp;action=showcomment">See comments</a></p>
    </xsl:when>
    <xsl:when test="$action='addcomment'">

      <script language="javascript" type="text/JavaScript">
        function funcsubmit()
        {
        <xsl:for-each select="/website/field[translate(@required,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')='TRUE' and @type!='editor']">
          if (document.getElementById('field<xsl:value-of select="@id"/>').value == ''){
          alert('You have to fill in the field \'<xsl:value-of select="@label"/>\'');
          document.getElementById('field<xsl:value-of select="@id"/>').focus();
          return false;}
        </xsl:for-each>
        return true;
        }
      </script>

     <form action="/default.aspx?pageid={//selectedpage}" target="_self" method="post" name="tempForm" onsubmit="return funcsubmit();">
        <table cellpadding="2" cellspacing="0" border="0" class="Comment">
          <input type="hidden" name="action" value="doaddcomment"/>

          <xsl:for-each select="/website/field">
		        <xsl:variable name="name"><xsl:value-of select="@id"/></xsl:variable>
		        <tr><td valign="top"><label for="field{$name}"><xsl:value-of select="@label"/></label></td><td>
		        <xsl:choose>
			        <xsl:when test="@type='text'">
				        <input id="field{$name}" name="field{$name}" size="{@size}" class="alminput"/>
			        </xsl:when>
			        <xsl:when test="@type='largetext'">
				        <textarea id="field{$name}" name="field{$name}" class="alminput" cols="{@cols}" rows="{@rows}"></textarea>
			        </xsl:when>
			        <xsl:when test="@type='checkbox'">
				        <xsl:element name="input">
					        <xsl:attribute name="type">checkbox</xsl:attribute>
					        <xsl:attribute name="name">field<xsl:value-of select="$name"/></xsl:attribute>
					        <xsl:attribute name="id">field<xsl:value-of select="$name"/></xsl:attribute>
					        <xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
				        </xsl:element>
			        </xsl:when>
		        </xsl:choose>
		        </td></tr>
	        </xsl:for-each>
	        <tr>
		        <td></td>
		        <td>
			        <input value="Send" type="submit" class="almknap"/>&#160;
			        <input type="button" value="Cancel" class="almknap" onclick="history.go(-1);"/>
		        </td>
	        </tr>
	        </table>
        </form>
    </xsl:when>
    <xsl:otherwise>
      <div class="CommentBlock">
      <xsl:choose>
        <xsl:when test="count(//commentItem[@pageid=$pageid and @active=1])>0">
          <a href="/default.aspx?pageid={//selectedpage}&amp;action=showcomment"><xsl:value-of select="count(//commentItem[@pageid=$pageid and @active=1])"/> Comment(s)</a>
        </xsl:when>
        <xsl:otherwise>
          <div style="float:left">
            No comments
          </div>
          <div style="float:right">
            <a href="/default.aspx?pageid={//selectedpage}&amp;action=addcomment">Add comment</a>
          </div>

        </xsl:otherwise>
      </xsl:choose>
      </div>
    </xsl:otherwise>
  </xsl:choose>

</xsl:template>
</xsl:stylesheet>