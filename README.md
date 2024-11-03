<div align="center">
   <h2> DaVinci Insights </h2>
</div>

<h3> Integrantes </h3>

- RM550341 - Allef Santos (2TDSPV)
- RM97836 - Débora Dâmaso Lopes
- RM551491 - Cassio Yuji Hirassike Sakai
- RM550323 - Paulo Barbosa Neto
- RM552314 - Yasmin Araujo Santos Lopes

- --------------------------------------------------

## Arquitetura

O projeto utiliza uma arquitetura monolítica, o que simplifica a comunicação interna entre os módulos, facilita o processo de desenvolvimento e reduz a complexidade dos testes. O sistema é composto por um backend em ASP.NET Core e um banco de dados MongoDB (Atlas).

- --------------------------------------------------

## Projeto  DaVinci

A API da DaVinci é desenvolvida em .NET com funcionalidades de CRUD, e treinamento de um modelo de análise de sentimentos usando ML.NET, e documentação via Swagger. Ela simula um ambiente de e-commerce para gerenciar clientes, produtos, feedbacks e compras, com foco em boas práticas de desenvolvimento.

## Serviço Externo

Foi implementada uma integração com o serviço de envio de e-mail **SendGrid**. Esse serviço é usado para enviar lembretes automáticos para os clientes que não deixaram feedbacks. Esse recurso adiciona valor ao ciclo de relacionamento com o cliente, incentivando avaliações e garantindo melhor atendimento.


## Práticas de Clean Code e SOLID Aplicadas

Durante o desenvolvimento da DaVinci API, foram aplicadas diversas práticas de Clean Code e princípios de SOLID, incluindo:

- Single Responsibility Principle (SRP): Cada classe tem uma responsabilidade única, como o FeedbackReminderService, que é responsável apenas por enviar lembretes de feedback.

- Dependency Injection: Todos os serviços e repositórios são injetados nas classes, facilitando testes e manutenção.

- Nomemclatura Clara: Métodos e variáveis possuem nomes que indicam claramente sua finalidade, aumentando a legibilidade do código.


## Testes Implementados

A API possui testes unitários, de integração e de sistema abrangentes, utilizando a biblioteca xUnit. Foram implementados testes para as seguintes funcionalidades:

- Testes unitários para os métodos de cada controlador (ex.: CostumerController, ProductController).

- Testes de integração para os repositórios (ex.: CostumerRepository, ProductRepository).

- Testes para o serviço de envio de e-mails para garantir que os lembretes de feedback são enviados corretamente.
<br/>

## ML.NET

O ML.NET foi utilizado para treinar um modelo de IA para análise de sentimentos em feedbacks de clientes, pensando em ajuda a classificar automaticamente os feedbacks como positivos ou negativos.

Para treinar o modelo com máxima acurácia possível (100), foi utilizado o dataset _sentimentos_produtos_normalizado.csv_ (disponível dentro da pasta MLMODELS > Data), que foi traduzido e utilizado normalização de texto para prepara-lo para o treinamento. 

O dataset contém comentários de consumidores e labels (1 para feedback positivo e 0 para negativo). Foi treinado na pasta da solução myMLApp.

Caso queira, você pode realizar uma previsão aleatória no terminal. No Visual Studio, após clonar o repositório, vá em myMLApp e procure pelo arquivo program.cs. Logo em seguida, substitua a frase entre aspas "" com a frase de seu interesse. Você verá uma linha assim: Col0 = "material de baixa qualidade parece barato". 

Após isso, clique no myMLApp e selecione a opção > depurar > iniciar sem depurar ou iniciar nova instância e você verá o resultado (positivo ou negativo) no terminal.

- --------------------------------------------------


## Endpoints

A API possui endpoints CRUD para gerenciar:

- **Clientes (Costumer)**
- **Produtos (Product)**
- **Feedbacks (Feedback)**
- **Compras (Purchase)**

- --------------------------------------------------

## Rodando a Aplicação (ALTERAR)

Passos:
1. Clonar o repositório:
   ```bash
   git clone https://github.com/debsdamaso/DaVinci-API.git
   ```

2. Executar testes:

    ```bash
    cd DaVinci-API
    dotnet test
    ```

3. Acessar o Swagger:
    ```bash
    https://localhost:7104/swagger/index.html
    ```

4. Testar Endpoints:
   ### Listar Clientes

   `GET` /api/costumer : Retorna os dados do cliente
   
   #### Exemplo de Resposta
   
   ```js
   [
   	{
     		"id": "605c72ef6b8f4e1f241c7f8a",
            "name": "João Silva",
            "email": "joao.silva@example.com"
   	}
   ]
   ```
   
   ### Detalhar Cliente
   
   `GET` /api/costumer/`{id}`
   
   Retorna os detalhes do cliente com o `id` informado no path.
   
   #### Exemplo de Resposta
   ```js
   // GET /api/costumer/{id}
   {
     	"id": "605c72ef6b8f4e1f241c7f8a",
        "name": "João Silva",
        "email": "joao.silva@example.com"
   }
   ```
   
   ### Cadastrar Cliente
   
   `POST` /api/costumer
   
   Cadastre um cliente com os dados enviados no corpo da requisição.
   
   #### Exemplo de Requisição
   ```js
   // Post /api/costumer
   {
       "id": "605c72ef6b8f4e1f241c7f8a",
        "name": "João Silva",
        "email": "joao.silva@example.com"
   }
   
   ```
   
   ### Atualizar Cliente
   `PUT` /api/costumer/`{id}`
   
   Atualiza os dados do cliente com o `id` informado no path, utilizando as informações do corpo da requisição
   
   #### Exemplo de Requisição
   ```js
   // PUT /api/costumer{id}
   {
       "id": "605c72ef6b8f4e1f241c7f8a",
        "name": "João Silva Atualizado",
        "email": "joao.silva.atualizado@example.com"
   }
   ```
   
   ### Apagar Cliente
   
   `DELETE` /api/costumer/`{id}`
   
   Apaga o cliente com o `id` informado no path

   ----------------------------------------

   ### Listar Feedbacks

   `GET` /api/feedbacks : Retorna a lista de feedbacks.
   
   #### Exemplo de Resposta
   
   ```js
   [
       {
           "id": "605c72ef6b8f4e1f241c7f8c",
            "customerId": "605c72ef6b8f4e1f241c7f8a",
            "productId": "605c72ef6b8f4e1f241c7f8b",
            "comment": "Ótimo produto!",
            "sentiment": "positivo"
       }
   ]
   ```
   
   ### Detalhar Feedback

   `GET` /api/feedbacks/`{id}` : Retorna os detalhes do feedback com o `id` informado no path.
   
   #### Exemplo de Resposta
   
   ```js
   // GET /api/feedbacks/1
   {
       "id": "605c72ef6b8f4e1f241c7f8c",
       "customerId": "605c72ef6b8f4e1f241c7f8a",
       "productId": "605c72ef6b8f4e1f241c7f8b",
       "comment": "Ótimo produto!",
       "sentiment": "positivo"
   }
   ```
   
   ### Cadastrar Feedback
   
   `POST` /api/feedbacks : Cadastra um feedback com os dados enviados no corpo da requisição.
   
   #### Exemplo de Requisição
   
   ```js
   // POST /api/feedbacks
   {
       "id": "605c72ef6b8f4e1f241c7f8c",
       "customerId": "605c72ef6b8f4e1f241c7f8a",
       "productId": "605c72ef6b8f4e1f241c7f8b",
       "comment": "Ótimo produto!",
       "sentiment": "positivo"
   }
    ```
   
   ### Atualizar Feedback
   
   `PUT` /api/feedbacks/`{id}` : Atualiza os dados do feedback com o `id` informado no path, utilizando as informações do corpo da requisição.
   
   #### Exemplo de Requisição
   
   ```js
   // PUT /api/feedbacks/{id}
   {
       "id": "605c72ef6b8f4e1f241c7f8c",
       "customerId": "605c72ef6b8f4e1f241c7f8a",
       "productId": "605c72ef6b8f4e1f241c7f8b",
       "comment": "Produto atualizado, melhor que antes!",
       "sentiment": "positivo"
   }
   ```

   ### Apagar Feedback

   `DELETE` /api/feedbacks/`{id}` : Apaga o feedback com o `id` informado no path.

   ----------------------------------------

   ### Listar Produtos

   `GET` /api/products : Retorna a lista de produtos.
   
   #### Exemplo de Resposta
   
   ```js
   [
       {
           "id": "605c72ef6b8f4e1f241c7f8b",
            "name": "Notebook",
            "description": "Notebook de alta performance",
            "price": 3500.0
       }
   ]
   ```

   ### Detalhar Produto

   `GET` /api/products/`{id}` : Retorna os detalhes do produto com o `id` informado no path.
   
   #### Exemplo de Resposta
   
   ```js
   // GET /api/products/{id}
   {
       "id": "605c72ef6b8f4e1f241c7f8b",
       "name": "Notebook",
       "description": "Notebook de alta performance",
       "price": 3500.0
   }
   ```

   ### Cadastrar Produto

   `POST` /api/products : Cadastra um produto com os dados enviados no corpo da requisição.
   
   #### Exemplo de Requisição
   
   ```js
   // POST /api/products
   {
       "id": "605c72ef6b8f4e1f241c7f8b",
       "name": "Notebook",
       "description": "Notebook de alta performance",
       "price": 3500.0
   }
   ```

   ### Atualizar Produto

   `PUT` /api/Produtos/`{id}` : Atualiza os dados do produto com o `id` informado no path, utilizando as informações do corpo da requisição.
   
   #### Exemplo de Requisição
   
   ```js
   // PUT /api/products/{id}
   {
       "id": "605c72ef6b8f4e1f241c7f8b",
       "name": "Notebook Atualizado",
       "description": "Notebook com mais memória",
       "price": 4000.0
   }
   ```

   ### Apagar Produto

   `DELETE` /api/products/`{id}` : Apaga o produto com o `id` informado no path.




