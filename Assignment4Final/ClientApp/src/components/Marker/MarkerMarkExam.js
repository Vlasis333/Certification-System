import { useNavigate, Link, useLocation } from "react-router-dom";
import React, { useEffect, useState, useContext } from "react";
import axios from 'axios';
import parse from 'html-react-parser';
import { Col, Row, Stack, Button, Table } from "react-bootstrap";
import { AiOutlineCheck, AiOutlineClose } from "react-icons/ai";

function MarkExam(props) {

    const navigate = useNavigate();
    const location = useLocation();

    const [exam, setExam] = useState(location.state.data);
    const initialScore = location.state.data.candidateScore;

    const incomingData = location.state.data;
    const role = location.state.role;

    useEffect(() => {
        // initialScore = exam.candidateScore;
        console.log(incomingData)
        setExam(incomingData)
        // console.log(exam.exam.certificateTitle)
        // console.log(exam.candidateExamAnswers)
    }, []);


    const handleChange = (event, queIndex) => {
        const { name, value, type, checked } = event.target;
        // console.log(name);
        // console.log(event.target.checked);
        // console.log("type", type);
        // console.log(value);
        if (type === 'checkbox') {
            setExam({
                ...exam, candidateExamAnswers: exam.candidateExamAnswers.map((canAn, Index) => {
                    if (Index === queIndex) {
                        return {
                            ...canAn, [name]: checked
                        };
                    } return canAn;
                })
            })
        }
        // if (initialScore !== exam.candidateScore) {
        // setExam({...exam , candidateScore: exam.candidateExamAnswers.filter(ans=> ans.isCorrectModerated).count()})
        console.log(exam.candidateExamAnswers.filter(ans => ans.isCorrectModerated === true).length)
        // }
        // const updatedExam = {...exam, isModerated: true};
        // setFinalScore();
        // exam.isModerated = true;
        console.log(exam)
        // setExam(exam)
    }
    //--------------------------------------------------//filters the text from the raw html
    function Replace(temp) {
        return (
            <td
                dangerouslySetInnerHTML={{
                    __html: temp,
                }}
            ></td>
        )
    }

    function ReplaceV2(temp) {
        return (
            <p>{parse(temp)}</p>
        )
    }
    //--------------------------------------------------

    const handleSubmit = async (canExamId) => {
        exam.isModerated = true;
        setExam(exam);

        await axios.put(`https://localhost:7196/api/Markers/mark/${canExamId}`, exam)
            .then(function (response) {
                console.log(response);
            })
            .catch(function (error) {
                console.log(error);
            });
            navigate(-1);
    }


    const setFinalScore = () => {

        console.log(exam.candidateScore)
        setExam({ ...exam, candidateScore: exam.candidateExamAnswers.filter(answer => answer.isCorrectModerated === false).length })
        console.log(...exam.candidateExamAnswers.filter(answer => answer.isCorrectModerated === true))
        console.log(...exam.candidateExamAnswers.filter(answer => answer.id === 1))
    }

    return (
        <div>
            {
                 role !== "qualitycontrol" && exam.isModerated !== true ?
                    <h1 class="display-3 text-center align-middle">Mark Exam</h1> :
                    <h1 class="display-3 text-center align-middle">Exam Marking Details</h1>
            }
            <div>
                <Row>
                    <Col xs={8}>
                    {exam.exam !== undefined && 
                        <h4>Title: {exam.exam.certificateTitle}</h4>
                    }
                    </Col>
                    <Col>
                        {initialScore !== exam.candidateScore && (
                            <div>
                                <Row>
                                    <Col>
                                        Initial Score (
                                        {(initialScore / exam.maxScore) * 100}%)
                                        :
                                    </Col>
                                    <Col>
                                        {initialScore}/{exam.maxScore}
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        Score After Marking (
                                        {(exam.candidateScore / exam.maxScore) *
                                            100}
                                        %) :
                                    </Col>
                                    <Col>
                                        {exam.candidateScore}/{exam.maxScore}
                                    </Col>
                                </Row>
                            </div>
                        )}
                    </Col>
                </Row>
            </div>
            <Table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Question</th>
                        <th>Marked As</th>
                    </tr>
                </thead>
                <tbody>
                    {exam.candidateExamAnswers.map((que, index) => (
                        <tr key={index}>
                            <td xs={1}>{index + 1}</td>
                            <td>
                                {Replace(que.questionText)}
                                <div>
                                    <ul>
                                        <li>Correct Option:{Replace(que.correctOption)}</li>
                                        <li>Choosen Option:{Replace(que.chosenOption)}</li>
                                    </ul>
                                    {/* <details className="display-6 fs-4">
                                                            <summary>
                                                                Click here if you want to see all the options
                                                            </summary>
                                                            {que.map((option)=> {
                                                                <ol>
                                                                    <li>

                                                                    </li>
                                                                </ol>
                                                            })}
                                                            </details> */}
                                    {/* //  ------- ------- ------- OPTIONS------- -------  ------- ------- */}
                                    <div>
                                        <hr />
                                        <div
                                            key={index}
                                            name={que.id}
                                            className="my-1 "
                                        >
                                            <Row>
                                                <details className="display-6 fs-4">
                                                    <summary>Options</summary>
                                                    <div className="card card-body ">
                                                        <div className="justify-content-end"></div>
                                                        <Table>
                                                            <thead>
                                                                <th>#</th>
                                                                <th>Text</th>
                                                                <th>
                                                                    Correct{" "}
                                                                </th>
                                                            </thead>
                                                            <tbody>
                                                                {que.question.options.map(
                                                                    (option,
                                                                        index) => (
                                                                        <tr key={index}>
                                                                            <td>
                                                                                {index+1}
                                                                            </td>
                                                                            <td>{ReplaceV2(
                                                                                option.text
                                                                            )}</td>
                                                                            <td>{option.correct ? (
                                                                                <AiOutlineCheck />) : (
                                                                                <AiOutlineClose />
                                                                            )}
                                                                            </td>
                                                                        </tr>
                                                                    )
                                                                )}
                                                            </tbody>
                                                        </Table>
                                                    </div>
                                                </details>
                                            </Row>
                                        </div>

                                        <hr />
                                    </div>
                                </div>
                                {/* //  ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- ------- */}
                            </td>
                            <td>
                                {que.isCorrectModerated}
                                <input
                                    type="checkbox"
                                    name="isCorrectModerated"
                                    label="Is the certificate available for puchase?"
                                    defaultChecked={que.isCorrectModerated}
                                    onChange={(event) =>
                                        handleChange(event, index)
                                    }
                                    disabled={
                                        exam.isModerated === true ||
                                            role === "qualitycontrol"
                                            ? true
                                            : false
                                    }
                                />
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
            <Stack gap={3}>
                {(role !== "qualitycontrol" && exam.isModerated !==true) && (
                    <Button onClick={() => handleSubmit(exam.id)}>
                        Save & Submit Marking
                    </Button>
                )}
                <Button
                    variant="dark"
                    className="d-grid col-12 mx-auto mb-2"
                    onClick={() => navigate(-1)}
                >
                    {" "}
                    Go Back{" "}
                </Button>
            </Stack>
        </div>
    );
}


export default MarkExam;
