# 🛠️ Guia de Compilação e Distribuição

Este guia fornece as instruções necessárias para transformar o código-fonte do **Roblox Script Executor** em um executável funcional para Windows.

## 📋 Pré-requisitos

Para compilar este projeto, você precisará instalar as seguintes ferramentas no seu Windows:

1.  **[.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0):** O kit de desenvolvimento necessário para compilar aplicações .NET 8.
2.  **Visual Studio 2022** (Com a carga de trabalho **"Desenvolvimento para desktop com .NET"**) ou **VS Code** com a extensão C# Dev Kit.

## 🚀 Passo a Passo para Compilação

Abra o terminal (PowerShell ou CMD) na raiz do projeto e execute os seguintes comandos:

### 1. Restaurar Dependências
Este comando baixa todos os pacotes NuGet necessários (MaterialDesign, AvalonEdit, CommunityToolkit.Mvvm).
```bash
dotnet restore
```

### 2. Compilar o Projeto
Execute a compilação em modo Release para otimizar o desempenho e gerar o executável final.
```bash
dotnet publish src/RobloxScriptExecutor.UI/RobloxScriptExecutor.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
```

### 3. Localização do Executável
Após a conclusão, o arquivo `.exe` estará localizado em:
`src/RobloxScriptExecutor.UI/bin/Release/net8.0-windows/win-x64/publish/RobloxScriptExecutor.UI.exe`

---

## 📦 Distribuição

Se você deseja distribuir a ferramenta:
1.  Crie uma pasta chamada `RobloxScriptExecutor`.
2.  Coloque o arquivo `RobloxScriptExecutor.UI.exe` dentro dela.
3.  Crie uma subpasta chamada `scripts` para que o usuário possa salvar seus arquivos `.lua`.
4.  Inclua o arquivo `plugin/Main.lua` e as instruções de instalação do plugin.
