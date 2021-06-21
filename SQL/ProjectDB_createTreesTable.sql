USE ProjectDB
GO
CREATE TABLE FamilyTrees
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FamilyName NVARCHAR(50) NOT NULL,
	FamilyTree XML(dbo.FamilyTreeSchemaCollection)
);

INSERT INTO FamilyTrees (FamilyName, FamilyTree) VALUES ('Nowak', '<?xml version="1.0" encoding="UTF-8"?>

<Family>
    <People>
        <Person id="1">
            <Names>
                Jan
            </Names>
            <Surname>
                Nowak
            </Surname>
            <Born>
                1970-01-01
            </Born>
            <Died>
                2020-01-01
            </Died>
        </Person>
        <Person id="2">
            <Names>
                Anna
            </Names>
            <Surname>
                Nowak
            </Surname>
            <Born>
                1970-01-01
            </Born>
        </Person>
        <Person id="3">
            <Names>
                Tomasz
            </Names>
            <Surname>
                Nowak
            </Surname>
            <Born>
                2000-01-01
            </Born>
            <Mother id="2"/>
            <Father id="1"/>
        </Person>
    </People>
    <Marriages>
        <Marriage husband="1" wife="2" date="1999-04-12"/>
    </Marriages>
</Family>');
