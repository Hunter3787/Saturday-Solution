import React from "react";
import {Col, Button, Form} from 'react-bootstrap';

function SubmitButton(){
    return(
    <Col>
        <Form.Group controlId="Submit">
            <Button type="submit">Submit Review</Button>
        </Form.Group>
    </Col>
    );
}

export default SubmitButton;