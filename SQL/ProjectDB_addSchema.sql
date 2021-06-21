USE ProjectDB
GO
CREATE XML SCHEMA COLLECTION FamilyTreeSchemaCollection AS
N'<xs:schema attributeFormDefault="unqualified" elementFormDefault="qualified" xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="Names" type="xs:string"/>
  <xs:element name="Surname" type="xs:string"/>
  <xs:element name="Born" type="xs:date"/>
  <xs:element name="Died" type="xs:date"/>
  <xs:element name="Mother">
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base="xs:string">
          <xs:attribute type="xs:int" name="id" use="required"/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>
  <xs:element name="Father">
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base="xs:string">
          <xs:attribute type="xs:int" name="id" use="required"/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>
  <xs:element name="Person">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref="Names"/>
        <xs:element ref="Surname"/>
        <xs:element ref="Born" minOccurs="0"/>
        <xs:element ref="Died" minOccurs="0"/>
        <xs:element ref="Mother" minOccurs="0" maxOccurs="1"/>
        <xs:element ref="Father" minOccurs="0" maxOccurs="1"/>
      </xs:sequence>
      <xs:attribute type="xs:int" name="id" use="required"/>
    </xs:complexType>
  </xs:element>
  <xs:element name="Marriage">
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base="xs:string">
          <xs:attribute type="xs:int" name="husband" use="required"/>
          <xs:attribute type="xs:int" name="wife" use="required"/>
          <xs:attribute type="xs:date" name="date"/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>
  <xs:element name="People">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref="Person" maxOccurs="unbounded" minOccurs="0"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="Marriages">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref="Marriage" maxOccurs="unbounded" minOccurs="0"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="Family">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref="People"/>
        <xs:element ref="Marriages"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>';