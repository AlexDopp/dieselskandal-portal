describe('Auftraege', () => {
  beforeEach(() => {
    // Einloggen vor jedem Test
    cy.visit('/login')
    cy.get('input[type="email"]').type('jane.doe@test.de')
    cy.get('input[type="password"]').type('1234')
    cy.get('button[type="submit"]').click()
    cy.url().should('include', '/dashboard')
  })

  it('HomeButtonNavigation', () => {
    cy.visit('/auftraege')
    cy.contains('Dieselskandal Portal').click()
    cy.url().should('include', '/dashboard')
  })

  it('AuftragAnlegenUndLöschen', () => {
    cy.visit('/auftraege/anlegen')
    cy.get('input[formControlName="hersteller"]').type('Toyota')
    cy.get('input[formControlName="modell"]').type('Corolla')
    cy.get('input[formControlName="baujahr"]').clear().type('2021')
    cy.get('input[formControlName="fahrgestellnummer"]').type('SB1K53AE90E123456')
    cy.get('input[formControlName="kennzeichen"]').type('KA-TC 789')
    cy.get('input[formControlName="kaufpreis"]').clear().type('22000')
    cy.get('input[formControlName="haendler"]').type('Toyota Autohaus')
    cy.get('input[formControlName="kaufdatum"]').type('2021-06-15')       // ISO Format für type="date"

    // Zeit damit Formular validiert
    cy.wait(500)

    cy.get('button[type="submit"]').click()

    cy.url().should('include', '/auftraege')

    cy.get('.dx-datagrid').should('be.visible')
    cy.get('.dx-data-row').should('have.length.greaterThan', 0)

    /* Debugging
    cy.get('.dx-data-row').contains('Toyota').parents('.dx-data-row')
      .then(($row) => {
        cy.log($row.html())
      })
    */
    cy.get('.dx-data-row').contains('Toyota').parents('.dx-data-row')
      .within(() => {
        cy.get('.dx-icon-trash').click()
      })

    cy.wait(500)
    cy.get('.dx-data-row').contains('Toyota').should('not.exist')
  })
})
