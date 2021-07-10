# PEC1
Practica PEC1 de l'assignatura de Programació de Videojocs en 2D.

El nom del jóc és: **The Kindness battle**

## Funcionament del joc
Vaig canviar la temàtica del joc perquè no fos igual a la resta, en comptes d'una batalla d'insults és una batalla de compliments, de "piropos" que li has de dir a l'altre jugador i qui aconsegueixi dir 3 compliments ben dits guanya la batalla de l'amabilitat.

El joc consisteix en 2 jugadors que es troben i comencen a dir coses maques de l'altre, la primera ronda es decideix qui la comença de manera aleatòria. A partir d'aquí, si al jugador li toca llançar el compliment, ha d'estar atent de no dir una resposta, ja que estan posades en la mateixa llista a l'hora de dir el compliment o la resposta. Per a cada compliment hi ha una resposta associada que és la que s'ha de respondre en cas que et diguin aquell compliment. Si a l'hora de començar la ronda (on el jugador ha de dir un compliment) diu una resposta, automàticament perd la ronda i se li dóna el punt de victòria i al jugador 2 que també començarà la següent ronda. 
Si el jugador diu correctament un compliment, al cap d'uns segons, el jugador 2 (ordinador) respondrà una resposta aleatòria entre totes les respostes que hi hagi. Això fa que com més compliments hi hagi per jugar, més respostes i serà més complicat tant pel jugador com per l'ordinador endevinar la resposta.
Si el jugador 2 diu una resposta errònia, donarà un punt de victòria al jugador i ell mateix començarà de nou la següent ronda. Però si diu la resposta correcte, el punt de la ronda el guanyarà el jugador 2 i ell començarà la ronda. Quan ell comença la ronda, diu un compliment aleatòriament perquè el jugador pugui contestar una resposta correcte.
El primer jugador que guanyi tres rondes haurà guanyat la partida i es podrà sentir orgullós per ser el més amable dels dos!

## Com está fet
Aquest projecte de Unity està creat des de zero, sense fer servir el "template" proporcionat. Hi ha tres escenes com s'especifica a l'enunciat: la primera d'inici per accedir al joc, el joc en si i la pantalla final que diu qui ha estat el guanyador i si vols tornar a jugar-hi.
El joc comença amb una simple animació que introdueix els dos jugadors a l'escena, un cop completada d'animació es decideix aleatòriament qui comença la partida i es posa en marxa la màquina d'estats. Si li toca començar al jugador, se l'activarà el panell on es poden fer o respondre compliments i haurà de seleccionar un compliment perquè si diu una resposta haurà perdut la ronda. 
El llistat de compliments i respostes s'agafa d'un fitxer extern on cada compliment i resposta estan ordenats i posats en una línia diferent per poder agafar-les totes, per exemple:
"Compliment 1"
"Resposta de compliment 1"
"Compliment 2"
"Resposta del compliment 2"

A l'hora de llegir aquest fitxer, creo 3 llistes: una per guardar-hi els compliments, una per guardar-hi les respostes i un altre on estàn tots barrejats que és la que farà servir el jugador. Separar en dos llistes els compliments i les respostes m'ajuda a la hora de comprovar si el jugador ha dit correctament un compliment o respòs bé o perquè l'ordinador triï un compliment o una resposta per dir.

Per escriure els compliments i les respostes vaig agafar idees de la web: [https://www.verywellmind.com/positivity-boosting-compliments-1717559](https://www.verywellmind.com/positivity-boosting-compliments-1717559)

El joc conté una imatge amb degradat pel fons i sprites pels dos personatges que vaig crear des de la web: [https://www.pixilart.com/draw](https://www.pixilart.com/draw)

La música i els sons del joc estàn agafats de la web gratuïta: [https://freesound.org/](https://freesound.org/)

Video demostratiu del joc: [https://www.youtube.com/watch?v=O2Y4eAc2KAk](https://www.youtube.com/watch?v=O2Y4eAc2KAk)

Moltes gràcies i que gaudiu del joc!

