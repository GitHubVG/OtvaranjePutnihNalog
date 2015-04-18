CREATE TABLE Odjel
(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	[Ime odjela] nvarchar(50) not null

);

CREATE TABLE Zaposlenici
(
 ID INT PRIMARY KEY IDENTITY NOT NULL,
 Ime nvarchar(50) NOT NULL,
 Prezime nvarchar(50) NOT NULL,
 Email nvarchar(50),
 Student BIT not null,
 IDOdjela INT FOREIGN KEY REFERENCES Odjel(ID) not null

);

INSERT INTO ODJEL
VALUES
('Ljudski resursi'),('Sistem inženjer'),('Programer'),('Dizajner');


ALTER TABLE Zaposlenici
add constraint  CK_Email CHECK(Email LIKE '%@%.%')

INSERT INTO Zaposlenici
VALUES 
('Iva','Ivić','ivaivic@gmail.com',0,3),
('Pero','Perić','peroperic@gmail.com',0,1),
('Ana','Anic','anaanic@yahoo.com',1,4),
('Marko','Marković','mmarkovic@yahoo.com',1,3),
('Maja','Majić','mmajic@gmail.com',0,2),
('Anita','Anitić','aniticanita@gmail.com',0,1),
('Ivan','Ivanić','ivanic@yahoo.com',0,2),
('Nataša','Natasic','natasic@yahoo.com',0,3),
('Ana','Anic','anaanic@yahoo.com',0,1),
('Bela','Belic','belabelic@gmail.com',0,3),
('Domagoj','Domagojević','domagojevic@hotmail.com',1,3),
('Petra','Petric','petrapetric@gmail.com',1,4),
('Jozo','Jozić','jozic@net.hr',0,4),
('Mirna','Mirnić','mirnicmirna@outlook.com.com',0,4),
('Filip','Filipović','filipfilipovic@gmail.com',0,3);


SELECT * FROM ODJEL;
SELECT * FROM Zaposlenici;

CREATE VIEW vwZaposleniciPoOdjelima
AS
SELECT t1.Ime, t1.Prezime, t1.Email, t2.[Ime odjela]
FROM Zaposlenici t1
JOIN Odjel t2
ON t1.IDOdjela=t2.ID;


select * from vwZaposleniciPoOdjelima;

create table PrivatnoVozilo
(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	RegistracijaPrivatnogVozila INT NOT NULL,
	Suvozac bit NOT NULL,
	IDPutnogNaloga INT FOREIGN KEY REFERENCES PutniNalog(ID) not null
);

CREATE TABLE TipPrijevoznogSredstva
(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	TipPrijevoznogSredstva nvarchar(20) not null
);

INSERT INTO TipPrijevoznogSredstva
values
('Javni prijevoz'),('Privatno vozilo'),('Službeno vozilo');


CREATE TABLE [PrijevoznaSredstva]
(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	PrijevoznoSredstvo nvarchar(50) NOT NULL,
	IDTipPrijevoznogSredstva INT foreign key REFERENCES TipPrijevoznogSredstva(ID) NOT NULL
	
);




CREATE TABLE PutniNalog
(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	[Podnositelj zahtjeva] nvarchar(100) not null,
	[Datum pocetka putovanja] DateTime NOT NULL,
	[Datum zavrsetka putovanja] DateTime NOT NULL,
	[IDPrijevoznogSredstva] INT FOREIGN KEY REFERENCES [PrijevoznaSredstva](ID),
	[Razlog dolaska] nvarchar(max) NOT NULL,
	[Relacija putovanja] nvarchar(max) NOT NULL,
	[Broj projekta] INT,
	[Smještaj] BIT NOT NULL
	
);

CREATE TABLE Smejstaj
(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	[Prvo noćenje] datetime NOT NULL,
	[Zadnje noćenje] datetime not null,
	[Dolazak u smještaj] datetime not null,
	[Odlazak iz smještaja] datetime not null,
	Komentar nvarchar(max) null,
	IDPutnogNaloga INT FOREIGN KEY REFERENCES PutniNalog(ID) not null

);



INSERT INTO PrijevoznaSredstva
VALUES
('Vlak',1),('Autobus',1),('Zrakoplov',1),('Službeno vozilo',3),('Osobno vozilo',2);

SELECT * FROM PrijevoznaSredstva;
SELECT * FROM TipPrijevoznogSredstva;

SELECT * FROM SMEJSTAJ;

SELECT * FROM PutniNalog order by ID desc;

select * from PrivatnoVozilo;

CREATE VIEW vwPutniNalog
as
SELECT t1.[Datum pocetka putovanja],t1.[Datum zavrsetka putovanja],t1.[Podnositelj zahtjeva],t1.[Razlog dolaska],t1.[Relacija putovanja], t2.Komentar,t2.[Prvo noćenje],t2.[Zadnje noćenje],t3.RegistracijaPrivatnogVozila,t3.Suvozac
from PutniNalog t1
join Smejstaj t2
on t1.ID=t2.IDPutnogNaloga
join PrivatnoVozilo t3
on t1.ID=t3.IDPutnogNaloga
