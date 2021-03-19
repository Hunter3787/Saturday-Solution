import React from "react";
import {Container, Row, Col, Button, Form, InputGroup, ButtonGroup} from 'react-bootstrap';

function ReviewForm(){
    return(
    <Col>
        <Form.Group controlId="WriteReview">
            <InputGroup.Prepend>
                <InputGroup.Text>Enter a Review</InputGroup.Text>
            </InputGroup.Prepend>
            <Form.Control as="textarea" aria-label="With textarea" />
        </Form.Group>
    </Col>
    );
}

export default ReviewForm;