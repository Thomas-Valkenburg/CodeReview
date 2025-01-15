// ReSharper disable UseOfImplicitGlobalInFunctionScope
// ReSharper disable PossiblyUnassignedProperty

/* global cy */

describe("User stories", () => {
  const email = "email@email.com";
  const password = "Password123!";

  const title = "Cypress Test title";

  function login() {
    const url = cy.url();

    cy.visit("/account/login");

    cy.get(".flex-grow-1 > .d-flex > :nth-child(1) > .form-control").type(email);
    cy.get(".flex-grow-1 > .d-flex > :nth-child(2) > .form-control").type(password);

    cy.get(".flex-row > .flex-grow-1 > .d-flex > .btn").click();

    cy.wait(1000);

    cy.visit(url.href ?? "/");
  }

  /*function logout() {
    const url = cy.url();
  
    try {
      cy.visit("/");

      cy.get(".dropdown > .d-none").click();
      cy.get(":nth-child(2) > .dropdown-item").click();
      
      cy.wait(1000);

      cy.visit(url.href ?? "/");
    } catch (e) {
      cy.visit(url.href ?? "/");
    }
  }*/

  beforeEach(() => {
    cy.visit("/")
  });

  it("Register", () => {
    cy.get('[href="/account/register"]').click();

    cy.url().should("include", "/account/register");

    cy.get(".flex-grow-1 > .d-flex > :nth-child(1) > .form-control").type(email);
    cy.get(".flex-grow-1 > .d-flex > :nth-child(2) > .form-control").type(password);

    cy.get(".flex-row > .flex-grow-1 > .d-flex > .btn").click();
  });

  it("Login", () => {
    login();

    cy.get(".dropdown > .d-none").click();

    cy.get(":nth-child(1) > .dropdown-item").should("have.text", email);
    cy.get(":nth-child(2) > .dropdown-item").should("have.text", "Logout");
  });

  it("Create Post", () => {
    login();

    cy.get(".flex-column > :nth-child(1) > :nth-child(2) > .btn").click();

    cy.get(".m-auto > .form-control").type(title);

    cy.get(".notranslate").type("Cypress Test content");

    cy.get(".m-auto > .btn-primary").click();

    cy.wait(500);
    
    cy.get("#search-bar").type(title);

    cy.get(".d-sm-block > .d-flex > .btn").click();

    cy.get('[href="/post/26"] > .card-body > .card-title').should("have.text", title);
  });

  it ("View Post", () => {
    cy.get('[href="/post/26"]').click();

    cy.get("h2").should("have.text", title);

    cy.get('.public-DraftStyleDefault-block > [data-offset-key="9omk4-0-0"] > span').should("have.text", "Cypress Test content");
  });

  it("Comment on Post", () => {
    login();

    cy.get('[href="/post/26"]').click();

    cy.wait(1000);

    cy.get(".notranslate").type("Cypress Test comment");

    cy.get(".flex-column > .btn-primary").click();

    cy.get('.public-DraftStyleDefault-block > [data-offset-key="7jvja-0-0"] > span').should("have.text", "Cypress Test comment");
  });
});

// ReSharper restore UseOfImplicitGlobalInFunctionScope
// ReSharper restore PossiblyUnassignedProperty