# Train-Reservation-.Net
Train and vagons reservation Api

Girdi adları:
 
Tren : Train,
Vagon : Vagons, 
Ad : Name, 
Kapasite : Capacity, 
DoluKoltukAdet : FilledSeats, 
RezervasyonYapilacakKisiSayisi : ReservationSize, 
KisilerFarkliVagonlaraYerlestirilebilir : AllowDifferentVagon

{
    "Train": {
        "Name": "Doğu Ekspresi",
        "Vagons": [
            {"Name":"Vagon 1", "Capacity":100, "FilledSeats":68},
            {"Name":"Vagon 2", "Capacity":90, "FilledSeats":50}
        ]
    },
    "ReservationSize": 5,
    "AllowDifferentVagon": true
}
