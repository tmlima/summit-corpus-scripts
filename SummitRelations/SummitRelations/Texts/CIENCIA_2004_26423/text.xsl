  <xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0">

          <xsl:output method="text" indent="no"
  omit-xml-declaration="yes"/>

          <xsl:strip-space elements="sentence paragraph text"/>

           

          <xsl:template match="text">

          <xsl:apply-templates/>

          </xsl:template>

           

          <xsl:template match="headline">

          <xsl:text>&lt;italic&gt;</xsl:text>

          <xsl:apply-templates/>

          <xsl:text>&lt;/italic&gt;</xsl:text>

          <xsl:text>

          </xsl:text>

          </xsl:template>

           

          <xsl:template match="paragraph">

          <xsl:apply-templates/>

          <xsl:text>

          </xsl:text>

          </xsl:template>

           

          <xsl:template match="sentence">

          <!--<xsl:value-of select="@id"/>-->

          <xsl:apply-templates/>

          <xsl:text></xsl:text>

          </xsl:template>

           

          <xsl:template match="signal">

          <xsl:text>&lt;</xsl:text><xsl:value-of
  select="@id"/><xsl:text>&gt;</xsl:text>
  
  <xsl:apply-templates/>
  
  <xsl:text>&lt;/signal&gt;</xsl:text>

          </xsl:template>

           

          <xsl:template match="word">

          <xsl:choose>

          <!-- Suppress leading space before full stop and comma -->

          <xsl:when test="child::text()='.' or child::text()=','">

          <xsl:apply-templates/>

          </xsl:when>

          <xsl:otherwise>

          <xsl:text> </xsl:text>

          <xsl:apply-templates/>

          </xsl:otherwise>

          </xsl:choose>

          </xsl:template>

          </xsl:stylesheet>