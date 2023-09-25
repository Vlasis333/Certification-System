import React, { useEffect, useState, useContext } from "react";
import { AuthenticationContext } from '../components/auth/AuthenticationContext'
import { useNavigate } from 'react-router-dom';
import { BsExclamationOctagonFill } from "react-icons/bs";
import { FaUniversity, FaBook, FaQuestion, FaUserAlt, FaCheckDouble, FaExclamation, FaUserGraduate } from "react-icons/fa";
import Authorized from "./auth/Authorized";
import axios from "axios";
import { getToken, getUserId } from "./auth/handleJWT";
import CertificateList from "./Certificate_CRUD/CertificateList";


function Home() {

  const navigate = useNavigate();
  const { update, claims } = useContext(AuthenticationContext);
  const [claim, setClaim] = useState(claims.filter(x => x.name === "role")[0]?.value);
  const [isNew, SetIsNew] = useState(false);

  const getUserEmail = () => {
    const regex = /^[^@]+/;
    // const result = email.match(regex)[0];
    return claims.filter((x) => x.name === "email")[0]?.value.match(regex)[0];
  };

  useEffect(() => {
    const token = getToken();
    if (token !== null) {

      axios.get(`https://localhost:7196/api/Candidate/${getUserId(token)}`).then((response) => {
        console.log(response.data)
        SetIsNew(false)
      }).catch(function(error) {
        console.log(error);
        SetIsNew(true)
      });
    }
  }, []);

  return (
    <div>
      {!claim ?
        <>
          {/* FIX:(akotro) Claims exist here but claim is undefined */}
          {console.log('claims', claims)}
          <div className="lead fs-2 text-center mb-4" >Welcome to the ErroRList Homepage!</div>
          <div className="lead fs-4 text-center mb-4">Here are the products you can purchase...(once Logged in...!)</div>
          <CertificateList />
        </>
        :
        <></>
      }

      <Authorized
        role="candidate"
        authorized={<>
          <div className="display-5 mb-3 text-center">
            Hello {getUserEmail()}, Welcome to the {claims.filter((x) => x.name === "role")[0]?.value} Homepage!
          </div>

          {isNew ?
            <div>
              <div className="lead fs-2 text-center mb-4">
                Please fill your details to continue
              </div>
              <div className="d-grid gap-3">
                <button class="btn btn-lg fs-2 btn-warning" type="button" onClick={() => navigate("/candidate/create")}>
                  Fill your details  &nbsp;&nbsp;<BsExclamationOctagonFill /></button>

              </div>
            </div> :
            <div>
              <div className="lead fs-2 text-center mb-4">
                Please use the buttons below to begin
              </div>
              <div class="d-grid gap-3">
                <button class="btn btn-lg fs-2 btn-outline-success" type="button" onClick={() => navigate("/certificate")}>
                  Certificates &nbsp;&nbsp;<FaBook /></button>
                <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("candidate/availableexams")}>
                  Examinations &nbsp;&nbsp;<FaUniversity /></button>
              </div>
            </div>
          }
        </>}

      />

      <Authorized
        role="admin"
        authorized={<>
          <div>
            <div className="display-5 mb-3 text-center">
              Hello {getUserEmail()}, Welcome to the {claims.filter((x) => x.name === "role")[0]?.value} Homepage!
            </div>
            <div className="lead fs-2 text-center mb-4">
              Please use the buttons below to begin ( or the Navbar on top! )
            </div>
            <div class="d-grid gap-3">
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/certificate")}>
                Certificates &nbsp;&nbsp;<FaBook /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/ExamsList")}>
                Examinations &nbsp;&nbsp;<FaUniversity /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/questions")}>
                Questions &nbsp;&nbsp;<FaQuestion /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/candidate")}>
                Candidates &nbsp;&nbsp;<FaUserGraduate /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/users")}>
                Users &nbsp;&nbsp;<FaUserAlt /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/marker")}>
                Marking &nbsp;&nbsp;<FaCheckDouble /></button>
            </div>
          </div>
        </>}
      />


      <Authorized
        role="qualitycontrol"
        authorized={<>
          <div>
            <div className="display-5 mb-3 text-center">
              Hello {getUserEmail()}, Welcome to the {claims.filter((x) => x.name === "role")[0]?.value} Homepage!
            </div>
            <div className="lead fs-2 text-center mb-4">
              Please use the buttons below to begin ( or the Navbar on top! )
            </div>
            <div class="d-grid gap-3">
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/certificate")}>
                Certificates &nbsp;&nbsp;<FaBook /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/marker")}>
                Marking &nbsp;&nbsp;<FaCheckDouble /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/questions")}>
                Questions &nbsp;&nbsp;<FaQuestion /></button>
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/candidate")}>
                Candidates &nbsp;&nbsp;<FaUserGraduate /></button>
            </div>
          </div>
        </>}
      />

      <Authorized
        role="marker"
        authorized={<>
          <div>
            <div className="display-5 mb-3 text-center">
              Hello {getUserEmail()}, Welcome to the {claims.filter((x) => x.name === "role")[0]?.value} Homepage!
            </div>
            <div className="lead fs-2 text-center mb-4">
              Please use the buttons below to begin ( or the Navbar on top! )
            </div>
            <div class="d-grid gap-3">
              <button class="btn btn-lg fs-2 btn-outline-primary" type="button" onClick={() => navigate("/marker")}>
                Mark an Exam &nbsp;&nbsp;<FaCheckDouble /></button>
            </div>
          </div>
        </>}
      />


    </div>


  );
}

export default Home;

