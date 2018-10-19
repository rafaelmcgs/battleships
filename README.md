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
  - **Míssil balístico** - cooldown: 0 - tamanho: **1** - dano: 15 - expõe a localização do navio
  - **Artilharia pesada** - cooldown: 1 - tamanho **2x2** - dano: 15
  - **Bombardeio** - cooldown: 3 - tamanho **1x7** - dano: 15
* O jogo acaba quando todos os **navios** de um jogador forem destruídos.

O projeto possuirá 3 cenas:
* Main
* Posicionamento
* Batalha

O projeto será desenvolvido para android e tela landscape fixa.

Prints:
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena1.jpg?raw=true)
![N|Solid](https://github.com/rafaelmcgs/battleships/blob/master/Referencias/prints/cena2.jpg?raw=true)
