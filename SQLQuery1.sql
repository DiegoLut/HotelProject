SELECT * FROM [Klient] 
SELECT * FROM [Pokoj] 
SELECT * FROM [RezerwacjaUsluga] 
SELECT * FROM [Rezerwacja] 
SELECT * FROM [Usluga]

INSERT INTO Usluga (Nazwa, opis, cena) VALUES('Sauna', 'Sauna po obiadku', '20')
INSERT INTO Rezerwacja (NumerPokoju, Email, DataZameldowania, DataWymeldowania, Rabat)
INSERT INTO RezerwacjaUsluga (RezerwacjaID, NazwaUslugi) VALUES(28, 'Sauna')



ALTER TABLE RezerwacjaUsluga
ALTER COLUMN CenaLaczna decimal(10,2) NULL;

DELETE FROM RezerwacjaUsluga

ALTER TABLE Rezerwacja ALTER COLUMN Rabat decimal(5,2) NULL
ALTER TABLE RezerwacjaUsluga ALTER COLUMN CenaLaczna decimal(10,2) NOT NULL
ALTER TABLE RezerwacjaUsluga ADD CONSTRAINT FK_NazwaUslugi_Nazwa FOREIGN KEY (NazwaUslugi) REFERENCES Usluga(Nazwa)

ALTER TABLE Usluga DROP CONSTRAINT FK_NazwaUslugi_Nazwa
ALTER TABLE Usluga DROP CONSTRAINT UQ_Nazwa

DELETE FROM Usluga WHERE UslugaID = 1

ALTER TABLE Usluga ADD CONSTRAINT UQ_UslugaID UNIQUE (UslugaID)

DELETE FROM Rezerwacja