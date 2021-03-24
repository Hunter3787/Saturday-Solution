import React, { useState } from "react";
import {Container, Row, Col, Button, Form} from 'react-bootstrap';


function ReviewsAndRatings(){

    const [message, setMessage] = useState({review: ''})

    const [star, setStar] = useState({selection: ''})

    if(message.review > "")
    {
        document.cookie = "message="+message.review;
    }

    if(star.selection > 0)
    {
        document.cookie = "selection="+star.selection;
    }

    // document.cookie = "selection="+star.selection;

    return(
        <Container>
            <Form>
                <Row>
                    <Col>
                        <Form.Group controlId="Select Rating">
                            <Form.Label>Select a Rating</Form.Label>
                                <Form.Control
                                as="select"
                                useref={star.selection}
                                type="text"
                                onChange={e=> setStar({...star, selection: e.target.value})}>
                                    <option value = "">Select a Rating</option>
                                    <option value = "1">1</option>
                                    <option value = "2">2</option>
                                    <option value = "3">3</option>
                                    <option value = "4">4</option>
                                    <option value = "5">5</option>
                                </Form.Control>
                        </Form.Group>
                    </Col>
                    <Col>
                        <Form.Group controlId="WriteReview">
                            <Form.Label>Enter a Review</Form.Label>
                            <Form.Control placeholder="type a review..."
                            as="textarea" 
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