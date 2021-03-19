import React from "react";
import {Container, Row, Col, Button, Form, InputGroup, ButtonGroup} from 'react-bootstrap';


function OneToFive(){
    return(
    <Col>
        <Form.Group controlId="1to5">
            <ButtonGroup aria-label="1to5scale">
                <Button>1</Button>
                <Button>2</Button>
                <Button>3</Button>
                <Button>4</Button>
                <Button>5</Button>
            </ButtonGroup>
        </Form.Group>
    </Col>
    );
}

export default OneToFive;