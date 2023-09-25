import React, { useState } from "react";
import { Button, Alert } from 'react-bootstrap';

const Errors = ({ error }) => {

    const [show, setShow] = useState(true);

    return error && error.response && error.response.data && error.response.data.errors ? (
        <>
            <Button variant="danger" onClick={() => setShow(!show)} style={{ marginBottom: '10px' }}>
                {show ? 'Minimize' : 'Show'}
            </Button>

            {show && (
                <Alert variant="danger">
                    <ul className="my-auto">
                        {
                            Object.entries(error.response.data.errors).map(([key, value]) => (
                                <li key={key}>{key}: {value[0]}</li>
                            ))
                        }
                    </ul>
                </Alert>
            )}
        </>
    ) : error ? (
        <>
            <Button variant="danger" onClick={() => setShow(!show)} style={{ marginBottom: '10px' }}>
                {show ? 'Minimize' : ' Show'}
            </Button>

            {show && (
                <Alert variant="danger">
                    <p>{error}</p>
                </Alert>
            )}
        </>
    ) : null;
};

export default Errors;