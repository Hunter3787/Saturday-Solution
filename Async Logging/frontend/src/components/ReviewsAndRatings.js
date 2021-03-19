import React, { useState } from "react";
import {Container, Row, Col, Button, Form, InputGroup, ButtonGroup} from 'react-bootstrap';


function ReviewsAndRatings(){

    const [message, setMessage] = useState({review: ''})

    const [star, setStar] = useState({selection: 0})

    if(message.review === "Hello")
    {
        document.cookie = "message="+message.review;
    }

    if(star.selection === 1)
    {
        document.cookie = "selection="+star.selection;
    }

    // document.cookie = "selection="+star.selection;

    return(
        <Container>
            <Form>
                <Row>
                    <Col>
                        <Form.Group controlId="1to5">
                            <ButtonGroup aria-label="1to5scale">
                                <Button
                                useref={star.selection}
                                type="number"
                                onClick={e => setStar({...star, selection: e.target.value = 1})}>1</Button>
                                <Button
                                useref={star.selection}
                                type="number"
                                onClick={e => setStar({...star, selection: e.target.value = 2})}>2</Button>
                                <Button
                                useref={star.selection}
                                type="number"
                                onClick={e => setStar({...star, selection: e.target.value = 3})}>3</Button>
                                <Button
                                useref={star.selection}
                                type="number"
                                onClick={e => setStar({...star, selection: e.target.value = 4})}>4</Button>
                                <Button
                                useref={star.selection}
                                type="number"
                                onClick={e => setStar({...star, selection: e.target.value = 5})}>5</Button>
                            </ButtonGroup>
                        </Form.Group>
                    </Col>
                    <Col>
                        <Form.Group controlId="WriteReview">
                            <InputGroup.Prepend>
                                <InputGroup.Text>Enter a Review</InputGroup.Text>
                            </InputGroup.Prepend>
                            <Form.Control as="textarea" 
                            aria-label="With textarea"
                            useref={message.review}
                            type="text"
                            onChange={e => setMessage({...message, review: e.target.value})}/>
                        </Form.Group>
                    </Col>
                    <Col>
                        <Form.Group controlId="Submit">
                            <Button type="submit">Submit Review</Button>
                        </Form.Group>
                    </Col>
                </Row>
                <h2>{message.review}</h2>
                <h2>{console.log(star.selection)}</h2>
            </Form>
        </Container> 
    );
}

export default ReviewsAndRatings;