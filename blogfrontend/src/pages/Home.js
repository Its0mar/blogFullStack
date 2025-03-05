import Logout from "../components/common/LogoutButton";

function Home() {

  let message = "Welcome to Zero Blog";


  return <div>
        {message}
        <Logout/>
       </div>;
}

export default Home;
