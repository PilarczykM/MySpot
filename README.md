# MySpot

An aplication for booking parking place.

## Busines requirements

* User can book only 1 of N parking place.
* Each parking spot have unique number.
* Parking spot can be booked only for next day.
   * Parking spot can be booked for one day within current week.
* Booked parking spot required vehicle number plate.
* New Policy:
  * CEOs, Managers have ability to book the best parking spots till 2PM, after that time normal employee can book those place.
* NEW special type of reservation for cleaning purpose.

## Tools

* .Net Core
* PostgreSQL

## Libs

* Humanizer
* Npgsql.EntityFrameworkCore.PostgreSQL
