# 🏡 ImovelExpress

## Descrição

O ImovelExpress é uma API para o sistema de cadastro de reservas de casas e apartamentos. Os clientes têm a opção de anunciar seu imóvel para aluguel ou procurar por imóveis disponíveis para aluguel.

## Recursos Principais

1. **Anunciar e Alugar:**
   - 🏠 Os clientes podem anunciar seus imóveis para aluguel.
   - 🔑 Os clientes podem alugar imóveis disponíveis.

2. **A API permite aos Proprietários:**
   - 🖼️ Cadastrar valores, fotos, comodidades, disponibilidade, etc.

3. **A API permite aos Locatários:**
   - 👀 Visualizar os imóveis que favoritaram para alugar.

4. **Cadastro de Usuários:**
   - 📝 Ambos os proprietários e locatários precisam realizar um cadastro para acessar os recursos de aluguel e anúncio.

5. **Restrição de E-mail:**
   - 📧 O e-mail de cadastro para aluguel não pode ser o mesmo de anúncio.

## Arquitetura e Testes

6. **Clean Architecture:**
   - 🏛️ A API foi construída seguindo os princípios da Clean Architecture.

7. **Testes Unitários e Mocker:**
   - 🧪 Foram realizados testes unitários e mocker com xUnit.

## Tecnologias, Frameworks e Bibliotecas Utilizadas

- ASP.NET ![ASP.NET](https://img.shields.io/badge/-ASP.NET-blue)
- .NET 7 ![.NET 7](https://img.shields.io/badge/-.NET%207-blue)
- AutoMapper ![AutoMapper](https://img.shields.io/badge/-AutoMapper-green)
- Swagger ![Swagger](https://img.shields.io/badge/-Swagger-orange)
- EntityFramework ![EntityFramework](https://img.shields.io/badge/-EntityFramework-purple) etc...

## Deploy e Armazenamento

8. **Azure Deployment:**
   - ☁️ A API foi implantada na Azure. [Link para o Swagger da API no Azure](https://apiimovelexpressproduction.azurewebsites.net/swagger)

9. **Blob Storage:**
   - 📁 O Blob Storage da Azure é utilizado para armazenar as imagens.

## Futuras Integrações

10. **Integração com APIs de Pagamentos:**
    - 💳 Futuramente, a API terá integração com APIs de pagamentos.

## Instruções para Clone do Projeto

- 📋 Clone o repositório:
    ```bash
    git clone https://github.com/jordanilson/APIImovelExpress.git
    ```

- 📦 Instale as dependências:
    ```bash
    cd APIImovelExpress
    dotnet restore
    ```

- ▶️ Execute o projeto:
    ```bash
    dotnet run
    ```

## Como Contribuir

11. **Contribuição:**
    - 🤝 Contribuições são bem-vindas!
