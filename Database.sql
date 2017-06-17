
create table VrstaProizvoda(
naziv_vrste varchar2(200),
opis varchar2(200),
constraint pk_vrsta primary key(naziv_vrste)
);

create table Proizvod(
idproiz int,
naziv varchar2(200),
cijena number,
kolicina int,
sezona date,
naziv_vrste varchar2(200),
constraint pk_proizvod primary key (idproiz),
constraint fk_vrsta foreign key(naziv_vrste) references VrstaProizvoda(naziv_vrste)
);

create table Klijent(
idk int,
ime varchar2(200),
prezime varchar2(200),
broj_telefona varchar2(20),
mail varchar2(200),
ukupno number default 0,
constraint pk_klijent primary key (idk)
);

create table Zaposleni(
jmbg char(13),
ime varchar2(200),
prezime varchar2(200),
broj_telefona varchar2(200),
korisnicko_ime varchar2(40) unique,
sifra varchar2(20),
constraint pk_zaposleni primary key (jmbg)
);
create table Prodaja(
idprod int,
datum date,
popust number default 0,
idk int,
jmbg char(13),
constraint pk_prodaja primary key(idprod),
constraint fk_klijent foreign key(idk) references Klijent(idk),
constraint fk_zaposleni foreign key(jmbg) references Zaposleni(jmbg)
);

create table Proprod(
idproiz int,
idprod int,
kolicina int,
constraint pk_proprod primary key(idproiz, idprod),
constraint fk_proizvod foreign key(idproiz) references Proizvod(idproiz),
constraint fk_prodaja foreign key(idprod) references Prodaja(idprod)
);







