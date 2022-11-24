describe('Test Registro 1', () => {
    it('No Debe poder registrar', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
        cy.get('.login-form-2 > form > :nth-child(1) > .form-control').type('Angel');
        cy.get('.login-form-2 > form > :nth-child(2) > .form-control').type('thor@gmail.com');
        cy.get(':nth-child(3) > .form-control').type('123456');
        cy.get(':nth-child(4) > .form-control').type('123456');
        cy.get('.login-form-2 > form > .d-grid > .btnSubmit').click();
        cy.get('#swal2-title').should('have.text', 'Error en la autenticación');
        cy.get('#swal2-html-container').should('have.text', 'El correo ya esta registrado');
        cy.get('.swal2-confirm').click();
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
    })
});


describe('Test Registro 2', () => {
    it('Debe poder registrar', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
        cy.get('.login-form-2 > form > :nth-child(1) > .form-control').type('Juan225');
        cy.get('.login-form-2 > form > :nth-child(2) > .form-control').type('juan225@gmail.com');
        cy.get(':nth-child(3) > .form-control').type('juan123');
        cy.get(':nth-child(4) > .form-control').type('juan123');
        cy.get('.login-form-2 > form > .d-grid > .btnSubmit').click();
        cy.get('.navbar-brand').should('include.text', 'Juan225');
    })
});

describe('Test Login 3', () => {
    it('No Debe pasar Login', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('juan22@gmail.com');
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('thor123d');
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();
      
        cy.get('#swal2-title').should('have.text', 'Error en la autenticación');
        cy.get('.swal2-confirm').click();
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
    })
});


describe('Test Login 4', () => {
    it('Debe pasar Login', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('thor@gmail.com', { delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('thor123', { delay: 80 });
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();

        cy.get('.navbar-brand').should('include.text', 'Thor');
    })
});

describe('Evento 1', () => {
    it('Debe poder crear un evento', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('thor@gmail.com',{ delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('thor123');
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();
   
        cy.get('.navbar-brand').should('include.text', 'Thor');
        cy.get('.btn-primary > .fas').click();
        cy.get('h1').should('include.text', 'Nuevo evento');
        cy.get(':nth-child(3) > .form-group > .form-control').type('juan22@gmail.com');
        cy.get('#search').submit();
        cy.get('.col-8 > .list-group-item').should('include.text', 'juan22@gmail.com');
        cy.get(':nth-child(1) > .react-datepicker-wrapper > .react-datepicker__input-container > .form-control').click();
        cy.get('.react-datepicker__day--024').click();
        cy.get('.react-datepicker__time-list > :nth-child(21)').click();
        cy.get(':nth-child(2) > .react-datepicker-wrapper > .react-datepicker__input-container > .form-control').click();
        cy.get('.react-datepicker__day--024').click();
        cy.get('.react-datepicker__time-list > :nth-child(26)').click();
        cy.get(':nth-child(4) > .form-control').type('pruebas con cypress');
        cy.get(':nth-child(5) > .form-control').type('Documentar');
        cy.get(':nth-child(6) > .btn > span').click();
        cy.get('.navbar-brand').should('include.text', 'Thor');
        cy.get('strong').should('include.text', 'pruebas con cypress');
    })
});


describe('Evento 2', () => {
    it('abrir evento evento y actualizar', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');

        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('thor@gmail.com',{ delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('thor123');
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();

        cy.get('.navbar-brand').should('include.text', 'Thor');

        cy.get('.rbc-event-label').dblclick();

        cy.get(':nth-child(5) > .form-control').clear();
        cy.get(':nth-child(5) > .form-control').type('documentar y capturas de pruebas corriendo', { delay: 80 });
        cy.get('.container > .btn').click();
        cy.get('#swal2-title').should('have.text', 'Actualizando evento...');
    })
});

describe('Evento 3', () => {
    it('Eliminar evento ', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');

        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('thor@gmail.com', { delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('thor123', { delay: 80 });
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();
    
        cy.get('.navbar-brand').should('include.text', 'Thor');
        cy.get('.rbc-event-content').click();
        cy.get('.btn-danger > .fas').click();
        cy.get('#swal2-title').should('have.text', 'Eliminando evento...');
    })
});

describe('Evento 4', () => {
    it('Debe poder crear un evento con participanter', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');
        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('thor@gmail.com',{ delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('thor123');
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();

        cy.get('.navbar-brand').should('include.text', 'Thor');
        cy.get('.btn-primary > .fas').click();
        cy.get('h1').should('include.text', 'Nuevo evento');
        cy.get(':nth-child(3) > .form-group > .form-control').type('ing.ricardo.alex@gmail.com');
        cy.get('#search').submit();
        cy.get(':nth-child(3) > .form-group > .form-control').type('tesismarin213@gmail.com');
        cy.get('#search').submit();

        cy.get(':nth-child(1) > .react-datepicker-wrapper > .react-datepicker__input-container > .form-control').click();
        cy.get('.react-datepicker__day--024').click();
        cy.get('.react-datepicker__time-list > :nth-child(21)').click();
        cy.get(':nth-child(2) > .react-datepicker-wrapper > .react-datepicker__input-container > .form-control').click();
        cy.get('.react-datepicker__day--024').click();
        cy.get('.react-datepicker__time-list > :nth-child(26)').click();
        cy.get(':nth-child(4) > .form-control').type('pruebas con cypress');
        cy.get(':nth-child(5) > .form-control').type('Documentar');
        cy.get(':nth-child(6) > .btn > span').click();
        cy.get('.navbar-brand').should('include.text', 'Thor');
    })
});

describe('Evento 5', () => {
    it('Debe poder aceptar notificaciones ', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');

        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('ing.ricardo.alex@gmail.com', { delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('ricardo123', { delay: 80 });
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();

        cy.get('.navbar-brand').should('include.text', 'Ricardo Alex');
        cy.get('.icon-button__badge').click();
        cy.get('.dropdown-item').click();
        cy.get('.btn-success').click();
        cy.get('#swal2-title').should('include.text', 'Guardando evento...');
    })
});

describe('Evento 6', () => {
    it('Debe poder rechazar notificaciones ', () => {
        cy.visit('http://localhost:5173')
        cy.get('.login-form-1 > h3').should('have.text', 'Ingreso');
        cy.get('.login-form-2 > h3').should('have.text', 'Registro');

        cy.get('.login-form-1 > form > :nth-child(1) > .form-control').type('tesismarin213@gmail.com', { delay: 80 });
        cy.get('.login-form-1 > form > :nth-child(2) > .form-control').type('marin123', { delay: 80 });
        cy.get('.login-form-1 > form > .d-grid > .btnSubmit').click();
 
        cy.get('.navbar-brand').should('include.text', 'Marin');
        cy.get('.icon-button__badge').click();
        cy.get('.dropdown-item').click();
        cy.get('.text-centar > .btn-danger').click();
        cy.get('#swal2-title').should('include.text', 'Rechazando...');
    })
});
