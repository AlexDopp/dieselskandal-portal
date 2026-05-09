describe('Login', () => {
  beforeEach(() => {
    cy.visit('/login')
  })

  it('LoginErfolgreich', () => {
    cy.get('input[type="email"]').type('jane.doe@test.de')
    cy.get('input[type="password"]').type('1234')
    cy.get('button[type="submit"]').click()
    cy.url().should('include', '/dashboard')
  })

  it('LoginFalschesPasswort', () => {
    cy.get('input[type="email"]').type('jane.doe@test.de')
    cy.get('input[type="password"]').type('FalschesPasswort')
    cy.get('button[type="submit"]').click()
    cy.url().should('include', '/login')
  })
})
