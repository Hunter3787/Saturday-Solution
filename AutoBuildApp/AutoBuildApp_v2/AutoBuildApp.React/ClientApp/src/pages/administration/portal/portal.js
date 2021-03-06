import React from 'react';
import "./portal.css";
import Nav from 'react-bootstrap/Nav';
import Button from 'react-bootstrap/Button'
import { Container, Row } from 'react-bootstrap';
import Col from 'react-bootstrap/Col'
/*


Elements on the page:
    Image buttons with name:
        User Analysis Dashboard
        User Management(?)
*/
function Portal(){
    return (



    <div className="portal-wrapper">
         <Nav.Link href="/user-analysis" /*hidden*/>User-Analysis</Nav.Link>
        
         <div className="mb-2">
             <Button variant="primary" size="lg"> Large button</Button>{' '}
              <Button variant="secondary" size="lg"> Large button </Button></div>

<Button onClick ={(e) =>{ e.preventDefault(); window.location.href="/user-analysis";}}
>UserAnalysis!</Button>


<Container>
<Row>
    <Col md={{ span: 8, offset: 5}}>
        {<Button onClick ={(e) =>
            { e.preventDefault(); window.location.href="/user-analysis";}}>UserAnalysis!</Button>}
            </Col>

    <Col md={{ span: 5, offset: 6}}>
        {<Button onClick ={(e) =>
            { e.preventDefault(); window.location.href="/user-account-management";}}>User Account Management</Button>}
            </Col>
</Row>


</Container>


    </div>


    );
}

export default Portal;