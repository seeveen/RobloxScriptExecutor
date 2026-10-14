# 🔌 Guia de Instalação do Plugin (Roblox Studio)

Para que o executor funcione, o Roblox Studio precisa de um "receptor" que escute os comandos vindos da aplicação Windows.

## 🛠️ Configurações Necessárias no Studio

Antes de instalar o plugin, você **DEVE** habilitar estas duas opções nas configurações do Roblox Studio, caso contrário, a execução será bloqueada por segurança:

1.  Abra o **Roblox Studio**.
2.  Vá em **File** $\rightarrow$ **Studio Settings**.
3.  Na aba **Security**, marque as seguintes opções:
    *   ✅ **Allow HTTP Requests** (Permite que o Studio fale com o nosso servidor local).
    *   ✅ **Enable LoadString** (Permite que o Studio transforme texto em código executável).

## 📥 Como Instalar o Plugin

Existem duas formas de instalar o `Main.lua`:

### Método A: Instalação Manual (Rápido para Testes)
1.  Abra o seu projeto no Roblox Studio.
2.  Abra o **Command Bar** (View $\rightarrow$ Command Bar).
3.  Copie todo o conteúdo do arquivo `plugin/Main.lua`.
4.  Cole no Command Bar e aperte **Enter**.
    *   *Nota: Isso instalará o plugin apenas para a sessão atual.*

### Método B: Instalação como Plugin Permanente
1.  Crie um novo script no Studio do tipo **Script** ou **LocalScript**.
2.  Cole o código do `plugin/Main.lua` nele.
3.  Clique com o botão direito no script no Explorer e selecione **"Save as Local Plugin..."**.
4.  Salve o arquivo na pasta de plugins do Studio.
    *   *Agora o executor iniciará automaticamente sempre que você abrir o Studio.*

---

## ⚠️ Limitações e Segurança

*   **Porta do Servidor:** O executor utiliza a porta `8080` por padrão. Se você tiver outro serviço usando essa porta, o servidor de bridge não iniciará.
*   **Ambiente de Teste:** O plugin foi desenhado para rodar no ambiente de simulação do Studio.
*   **Caminhos de Arquivo:** A aplicação salva scripts na pasta `/scripts` relativa ao executável.
