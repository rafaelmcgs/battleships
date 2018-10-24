# battleships

Clássico jogo de estratégia naval, onde os jogadores tentam acertar os navios do oponente no escuro.

Nesta versão serão acrescentados novos tipos de ataques e navios.
O jogo possuirá um unico modo:
* Duelo entre jogadores

As regras serão:
* Sempre **2** participantes
* Cada participante possuirá um tabuleiro de **15x15** para posicionar os navios.
* Cada jogador possuirá as seguintes peças para posicionar:
  - **4 boias** – 1x1 – Não possui ataque;
  - **3 Corvetas** – 1x2 – Possui o ataque de artilharia simples;
  - **5 Fragatas** – 1x3 – Possui o ataque de artilharia simples e o míssil balístico;
  - **2 Cruzador** – 1x4 – Possui o ataque de artilharia simples e o artilharia pesada;
  - **1 Porta aviões** – 1x5 – Possui o ataque de artilharia simples e o bombardeio.
* Os participantes atacarão **alternadamente em turnos** e em cada turno o jogador poderá lançar **quantos ataques quiser**, contanto que estejam **disponíveis**. Os ataques são:
  - **Artilharia Simples** - cooldown: 0 - tamanho: **1** - dano: somatório da quantidade de navios
  - **Míssil balístico** - só pode ser usado 1 vez por cada navio - tamanho: **3x3** - dano: 10 - expõe a localização de uma fragata por 1 rodada
  - **Artilharia pesada** - cooldown: 2 - tamanho **1** - dano: 15 - expoe a localização completa do navio inimigo por 1 rodada
  - **Bombardeio** - cooldown: 2 - tamanho **1x7** - dano: 10
* O jogo acaba quando todos os **navios** de um jogador forem destruídos.

O projeto possuirá 3 cenas:
* Main
* Posicionamento
* Batalha
* Vitoria / resultado da batalha

O projeto será desenvolvido para android e tela landscape fixa.

Programas utilizados no projeto:
* Unity
* TexturePacker
* Photoshop
* Illustrator
* Audition
* Premiere

Nota para avaliador:
* Nota-se que os ataques estão totalmente vinculados aos navios
* Para alterar a quantidade de navios no tabuleiro vá na cena **SetShips** e altere o GameObject **navios**.
  - Adicione, remova ou mova os prefabs de navios ja criados e configurados
  - Snap o navio corretamente  
* Para alterar regras da batalha, como dano e countdown, vá na cena **Battle**, GameObject "BattleManager" e modifique as variaveis do script anexado.


Prints:
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena1.jpg?raw=true)
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena2.jpg?raw=true)
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena3.jpg?raw=true)
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena4.png?raw=true)
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena5.png?raw=true)

