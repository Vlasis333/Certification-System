import axios from "axios"

import { useState, useEffect, useContext } from "react";
import { Form, Row, Col, Button, Accordion } from "react-bootstrap";
import { useNavigate } from "react-router";
import { AuthenticationContext } from '../auth/AuthenticationContext'
import BackButton from "../Common/Back";
import MarkerList from "../Marker/MarkerList";


function AssignToMarker() {
    const navigate = useNavigate();
    const { update, claims } = useContext(AuthenticationContext);
    const [markers, setMarkers] = useState([]);
    const [notAssignedExams, setNotAssignedExams] = useState([]);
    const [assignedExams, setAssignedExams] = useState([]);
    const [assignExam, setAssignExam] = useState({ marker: [] });
    // const [ReAssignExam, setReAssignExam] = useState({marker:[]});

    useEffect(() => {
        axios.get(`https://localhost:7196/api/Markers`).then((response) => {
            console.log(response.data.data)
            setMarkers(response.data.data);
        }).catch(function(error) {
            console.log(error);
        });
        axios.get(`https://localhost:7196/api/Markers/getallcandidateexams/`).then((response) => {
            console.log(response.data.data)
            const givenExams = (response.data.data.filter(x => x.result === true || x.result === false));
            setNotAssignedExams(givenExams.filter(exam => exam.marker === null || exam.marker === undefined))//swsto
            // )
            setAssignedExams(givenExams.filter(exam => (exam.marker !== undefined) && (exam.isModerated !== true)))

        }).catch(function(error) {
            console.log(error);
        });
    }, []);

    const convertDateToString = (date) => {
        let kati = new Date(date);
        let formattedDate = kati.toLocaleDateString().substr(0, 10);
        return formattedDate;
    };

    const handleChange = (event) => {

        // edit the CandidateExam to add the Marker ID
        const { name, value, type, id } = event.target;
        console.log(event.target);
        console.log(event.target.id);
        // console.log(name);
        // console.log(type);
        // console.log(value);
        // console.log(exams.find(item => item.id === Number(value)))
        if (id === "assign") {
            if (name === "id") {
                setAssignExam({ ...assignExam, [name]: value })
            } else if (name === "marker") {
                setAssignExam({ ...assignExam, [name]: { appUserId: value } })
                // setnewCandidateExam({ ...newCandidateExam,  })
                console.log(value)
            }
        } else {
            if (name === "id") {
                setAssignExam({ ...assignExam, [name]: value })
            } else if (name === "marker") {
                setAssignExam({ ...assignExam, [name]: { appUserId: value } })
                // setnewCandidateExam({ ...newCandidateExam,  })
            }
        }
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
        console.log(assignExam)
        await axios.put(`https://localhost:7196/api/Markers/assign/${assignExam.id}`, assignExam)
            .then(function(response) {
                console.log(response);
            })
            .catch(function(error) {
                console.log(error.messages);
            });
        document.getElementById("assignForm").reset();
        document.getElementById("re-assignForm").reset();
        navigate(-1);
    }

    return (
        <div>
            <h1 class="display-3 text-center align-middle">Assign Exams For Marking</h1>
            <div className="my-auto py-5 lead">
                <Accordion defaultActiveKey="0">
                    <Accordion.Item eventKey="0">
                        <Accordion.Header >
                            <div class="display-5">
                                Assign To Marker
                            </div>
                        </Accordion.Header>
                        <Accordion.Body>
                            <Form onSubmit={handleSubmit} id="assignForm">
                                <Row >
                                    <Col xs={4}>
                                        <Form.Group >
                                            <Form.Label>Select Marker</Form.Label>
                                            <Form.Select as="select" name="marker" id="assign"
                                                value={assignExam.marker.appUserId}
                                                onChange={handleChange}
                                                required>
                                                <option value="" hidden>Please choose a Marker </option>
                                                {markers.map((marker, index) =>
                                                    <option key={index}
                                                        value={marker.appUserId}
                                                    >{marker.appUser.email}</option>
                                                )}
                                            </Form.Select>
                                        </Form.Group>
                                    </Col>
                                    <Col>
                                        <Form.Group >
                                            <Form.Label>Select Exam</Form.Label>
                                            <Form.Select as="select" name="id" id="assign"
                                                value={assignExam.exam}
                                                onChange={handleChange}
                                                required>
                                                <option value="" hidden>Please choose an exam </option>
                                                {notAssignedExams.map((exam, index) =>
                                                    <option key={index}
                                                        value={exam.id}
                                                    >{exam.candidate.firstName}&nbsp;
                                                        {exam.candidate.lastName}&nbsp;|&nbsp;
                                                        {exam.examDate ?
                                                            <>
                                                                Exam date : {convertDateToString(exam.examDate)}&nbsp;|&nbsp;
                                                            </> :
                                                            <>
                                                                Exam date : {convertDateToString(exam.examDate)}&nbsp;|&nbsp;
                                                            </>
                                                        }
                                                        Voucher : {exam.voucher}
                                                    </option>
                                                )}
                                            </Form.Select>
                                        </Form.Group>
                                    </Col>
                                </Row>
                                <Button variant="success" className='d-grid gap-2 col-6 mx-auto py-2 my-4' type="submit" >
                                    Save & Assign
                                </Button>
                            </Form>
                        </Accordion.Body>
                    </Accordion.Item>
                    <Accordion.Item eventKey="1">
                        <Accordion.Header>
                            <div class="display-5">
                                Re-Assign to another Marker
                            </div>
                        </Accordion.Header>
                        <Accordion.Body>
                            <Form onSubmit={handleSubmit} id="re-assignForm">
                                <Row>
                                    <Col xs={4}>
                                        <Form.Group >
                                            <Form.Label>Select Marker</Form.Label>
                                            <Form.Select as="select" name="marker" id="reassign"
                                                value={assignExam.marker.appUserId}
                                                onChange={handleChange}
                                                required>
                                                <option value="" hidden>Please choose a Marker </option>
                                                {markers.map((marker, index) =>
                                                    <option key={index}
                                                        value={marker.appUserId}
                                                    >{marker.appUser.email}</option>
                                                )}
                                            </Form.Select>
                                        </Form.Group>
                                    </Col>
                                    <Col>
                                        <Form.Group >
                                            <Form.Label>Select Exam</Form.Label>
                                            <Form.Select as="select" name="id" id="reassign"
                                                value={assignExam.exam}
                                                onChange={handleChange}
                                                required>
                                                <option value="" hidden>Please choose an exam </option>
                                                {assignedExams.map((exam, index) =>
                                                    <option key={index}
                                                        value={exam.id}
                                                    >{exam.candidate.firstName}&nbsp;
                                                        {exam.candidate.lastName}&nbsp;|&nbsp;
                                                        {exam.examDate ?
                                                            <>
                                                                Exam date : {convertDateToString(exam.examDate)}&nbsp;|&nbsp;
                                                            </> :
                                                            <>
                                                                Exam date : {convertDateToString(exam.examDate)}&nbsp;|&nbsp;
                                                            </>
                                                        }
                                                        Voucher : {exam.voucher}
                                                    </option>
                                                )}
                                            </Form.Select>
                                        </Form.Group>
                                    </Col>
                                </Row>
                                <Button variant="success" className='d-grid gap-2 col-6 mx-auto py-2 my-4' type="submit" >
                                    Save & re-Assign
                                </Button>
                            </Form>
                        </Accordion.Body>
                    </Accordion.Item>
                </Accordion>
                <BackButton />
            </div></div>
    );
}

export default AssignToMarker
