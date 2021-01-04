# Projeto 2 - Pac-Man

## Grupo 01

|     Nome      |  Número   |   GitHub   |
| :-----------: | :-------: | :--------: |
| Pedro Marques | a21900253 | pmarques93 |
|  Luiz Santos  | a21901441 | JundMaster |

## Repositório git

[Repositório GitHub](https://github.com/pmarques93/Pacman)

## Tarefas realizadas no exercício

|         Luiz Santos          |            Pedro Marques            |
| :--------------------------: | :---------------------------------: |
|     Input, `Scene` base      | `GameObject` base, `Component` base |
|        Console Render        |           Move Component            |
|          Collisions          |             Collisions              |
|       Map, Map render        |                 Map                 |
|  Chase Behaviours (ghosts)   |                Score                |
|  Scatter Movement (ghosts)   |              Main Menu              |
| Frightened Movement (ghosts) |   Lives, High Score, Fruit Spawn    |
|   Eaten Movement (ghosts)    |              GameOver               |
|          Clean code          |             Clean code              |
|        UML / Read Me         |               Read Me               |

## Arquitetura da Solução

Este projeto é uma implementação do jogo [*Pac-man*](
    https://en.wikipedia.org/wiki/Pac-Man) em consola. Como base do projeto,
utilizámos o seguinte [*game engine*](
    https://github.com/fakenmc/CoreGameEngine).

### Funcionamento do Programa

![funcionamento do programa](Images/funcionamento_do_programa.png)

### Jogo

Ao iniciar a aplicação, é aresentado um menu principal ao utilizador. Este
menu apresenta as regras, a explicação dos icons, *high score* e controlos.
Após selecionar **Start Game**, é feita a criação de um nível do *pac-man*.
Ao terminar o nível (carregar no escape, perder 3 vidas ou comer todas as
*foods*), o jogo regista um novo *highscore* e volta para o menu inicial.

### *Highscore*

O **highscore** é obtido através do score atual no jogo, que é incrementado ao
comer **foods**, **fruits** e **ghosts** em *frightened mode*.

### *Foods*, *Fruits* e *PowerPills*

Estes 3 tipos de comida sobem o *score* ao jogador. As **foods** são
representadas com um `.` no mapa, sendo estas o tipo de comida principal
para terminar o jogo. As **fruits**, representadas com um `F` *spawnam* de x em
x segundos e têm o propósito de aumentar a pontuação, não sendo obrigatório o
consumo das mesmas para terminar o jogo. As **PowerPills**, representadas com
`PUP`, são um tipo de comida que troca o movimento dos *ghosts* do tipo de
movimento atual para **frightened mode** durante x segundos, sendo possível
comer os *ghosts* durante este tempo e subir a pontuação.

### *Ghosts*

Os *ghosts* têm três tipos de movimento, sendo estes o **scatter movement**,
em que o *ghost* faz uma patrulha num local definido e que, de x em x segundos
é alterado para **chase movement**; o **chase movement**, em que o ghost se
movimenta para uma certa coordenada consoante a posição do *pac-man* ou de
outros *ghosts*, alterando também para **scatter movement** após x segundos;
o **frightened movement**, em que o *ghost* anda aleatoriamente pelo mapa,
sendo que o *ghost* passa para este movimento após o *pac-man* consumir uma
*PowerPill*; e o **eaten movement** em que o *ghost* foge para a casa inicial,
sendo que após chegar à sua posição, o movimento faz *reset* para o tipo de
movimento inicial.

Os *ghosts* são representados com uma cor e uma letra, servindo para identificar
os mesmos. Quando estão em **frightened mode** a cor do seu quadrado fica
branca e quando estão em **eaten mode** a cor desaparece e fica apenas a sua
letra até chegarem à casa inicial.

Ao colidir com um *ghost* em **scatter movement** ou **chase movement** o
*pac-man* perde 1 vida, enquanto que ao colidir com um *ghost* em
**frightened mode**, é incrementada pontuação ao *score* e o *ghost* altera o
seu tipo de movimento para **eaten movement** e foge para a casa inicial.

### Colisões

Cada posição do mapa e as entidades nele presentes têm uma `Cell`, que serve
para definir o que acontece quando o *pac-man* colide com algum objeto. Ao
colidir com **foods**, **fruits**, **powerpills** e **ghosts** (dependendo do
tipo de movimento em que se encontram) são disparados eventos com alguma
consequência no jogo, como referido nos parágrafos anteriores.

### Vidas

Ao iniciar o jogo, o *pac-man* tem 3 vidas, perdendo estas vidas ao colidir
com *ghosts* em **scatter movement** ou **chase movement**. Após perder as 3
vidas, o jogo volta para o menu inicial.

### Algoritmos

#### Update method e Component Pattern

Para este projeto utizámos uma interface `IGameObject`, que dá origem à classe
abstrata `Component`, classe base dos componentes existentes no jogo, e à classe
`GameObject`, classe que cria todos os *game objects* no jogo.
A interface mencionada é constituída por métodos como o `Start`, `Update` e
`Finish`. Cada `GameObject` (que implementa `IGameObject`) contém uma coleção
com vários componentes, e, nos seus métodos `Start`, `Updated` e `Finish` vai
correr os métodos `Start`, `Update` e `Finish` de todos estes componentes.

Estes mesmos `GameObject` são adicionados a um dicinário de `IGameObject` na
classe `Scene` e, através do método `GameLoop`, todos estes `IGameObjects`
são executados uma vez inícialmente com o método `Start`, uma vez no fim
com o método `Finish`, e constantemente durante um *while*, com o método
`Update`, correndo assim todos os componentes de todos os `GameObject`
inseridos na scene.

De modo a serem adicionados a este dicionário, existe o método `AddGameObject`.
A utilização do mesmo pode ser observado na classe `MenuCreation` e
`LevelCreation`, nos seus métodos `AddGameObjectsToScene`, que é corrido no
final do método `Generate Scene`, depois de serem criados os `GameObject` do
jogo e todos os seus componentes.

#### *Strategy Pattern* e *Movement Behaviours*

Para a criação de movimentos, decidimos criar uma interface `IMovementBehaviour`
com um método `Movement`. Esta interface é implementada por qualquer tipo de
movimento que exista no jogo, sendo uma base importante para o controlo de
movimento dos `Ghosts`, sendo que permite a alteração do seu tipo de
movimento em tempo real.

Para execução do mesmo, existe um componente `MoveComponent`, que é adicionado
a um `GameObject` (como referido anteriormente), sendo que esta classe contém
uma variável `IMovementBehaviour` e um método `AddMovementBehaviour` que nos
permite adicionar um tipo de movimento a esta variável. Este componente vai
então correr no seu `Update`, o método `Movement` da sua variável de
`IMovementBehaviour`.

#### Inteligência Artificial

Os `Ghosts` movimentam-se pelo mapa de diferentes maneiras, dependendo de qual
modo de movimento estão, como foi referido anteriormente.

Em qualquer um dos modos, o movimento dos `Ghosts` basea-se sempre em
*targeting*, que funciona da seguinte forma - uma posição no mapa é selecionada,
e o `Ghost` vai escolher a posição adjacente a ele no mapa (cima, baixo,
direita ou esquerda) de modo a que, quando estiver nessa posição, a distância
absoluta entre o ele e o seu *target* seja reduzida.

Independente do modo de movimento em que estiverem, os `Ghosts`, de modo geral
não podem virar para trás, andar contra as paredes e nem entrar na
`Ghost House` - posição inicial dos `Ghosts` no mapa.

A exceção à regra de não andar para trás aplica-se quando um `Ghost` muda de um
modo de movimento para outro. Quando isto acontece, ele vira para trás e começa
a movimentar-se normalmente, segundo os critérios do novo modo de movimento.

##### *Chase Mode*

Neste modo de movimento, cada `Ghost` tem uma maneira própria de posicionar o
seu *target* no mapa, levando com que todos tenham movimentos bastante
diferentes e com isto, trabalhem, relativamente bem, em conjunto.

###### Blinky

Blinky é o fantasma vermelho, e o seu *target* é o mais simples - a posição
exata na qual o Pacman está atualmente.

###### Pinky

Pinky é o fantasma rosa, e o seu *target* é posicionado 4 unidades à frente do
Pacman. Excepto quando este está virado para cima, quando este é o caso o
*target* é posicionado 4 unidades para cima e 3 para o lado.

###### Inky

Inky é o fantasma azul, e o seu *target* é calculado pelo vetor entre a posição
do Pacman e a posição do `Ghost` blinky rodado 180º em torno da posição do
Pacman.

###### Clyde

Clyde é o fantasma amarelo, e o seu *target* está na posição em que o Pacman
está atualmente. Entretanto, se a distância absoluta entre o Clyde e o Pacman
for igual ou inferior a 4, o *target* passa a estar posicionado no canto
inferior direito do mapa.

#### *Scatter Mode*

Quando estão em *scatter mode*, o *target* de cada um dos `Ghosts` é
posicionado num dos cantos do mapa.

#### *Frightened Mode*

Quando os `Ghosts` estão em *frightened mode* os seus possíveis *targets* são
os cantos do mapa e são selecionados aleatoriamente.

#### *Eaten Mode*

Quando estão em *eaten mode* o *target* dos `Ghosts` é posicionado na saída
da *ghost house.*

### Diagrama UML

O seguinte diagrama é uma representação gráfica da estrutura de classes do
programa.

![Diagrama UML](UML.png)

## Referências

- [*Game Engine* base](
https://github.com/fakenmc/CoreGameEngine)

- [Pac-Man Rules](
https://en.wikipedia.org/wiki/Pac-Man)

- [Pac-Man Ghost AI](
https://www.youtube.com/watch?v=ataGotQ7ir8&feature=youtu.be)

- [C# Timers](
https://docs.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-5.0)

- [.NET API](https://docs.microsoft.com/en-us/dotnet/api/)
