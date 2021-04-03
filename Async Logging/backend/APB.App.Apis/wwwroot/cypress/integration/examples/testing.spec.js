/// <reference types="cypress" />

context('Actions', () => {
    beforeEach(() => {
      cy.visit('https://localhost:44317/')
    })

    it('.type() - type into a DOM element', () => {

        cy.get('#add-username')
        .type('fake username').should('have.value', 'fake username')

              // .type() with special character sequences
        .type('{leftarrow}{rightarrow}{uparrow}{downarrow}')
        .type('{del}{selectall}{backspace}')

        // .type() with key modifiers
        .type('{alt}{option}') //these are equivalent
        .type('{ctrl}{control}') //these are equivalent
        .type('{meta}{command}{cmd}') //these are equivalent
        .type('{shift}')

        // Delay each keypress by 0.1 sec
        .type('slow.typing@email.com', { delay: 100 })
        .should('have.value', 'slow.typing@email.com')

    })

    it('.type() - type into a DOM element', () => {

        cy.get('#add-message')
        .type('fake message').should('have.value', 'fake message')

              // .type() with special character sequences
        .type('{leftarrow}{rightarrow}{uparrow}{downarrow}')
        .type('{del}{selectall}{backspace}')

        // .type() with key modifiers
        .type('{alt}{option}') //these are equivalent
        .type('{ctrl}{control}') //these are equivalent
        .type('{meta}{command}{cmd}') //these are equivalent
        .type('{shift}')

        // Delay each keypress by 0.1 sec
        .type('fake message', { delay: 100 })
        .should('have.value', 'fake message')

    })

    it('.clear() - clears an input or textarea element', () => {
        // https://on.cypress.io/clear
        cy.get('#add-message').type('Clear this text')
          .should('have.value', 'Clear this text')
          .clear()
          .should('have.value', '')
      })
})