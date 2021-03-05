import "./about.css";

function About() {
    var privacyPolicy =<a href="/privacy-policy">privacy policy</a>;
    
    return (
        <div className="about-box">
            <h2>Who are we?</h2>
            <p>This website is created and published by a group of college seniors attempting to get some real world development experience.</p>
            <h2>What is this website?</h2>
            <p>This website is a tool to help users come up with custom personal computer (PC) build concepts using our recommendation algorithm.
                <br/>We provide the recommendation so you don't have to do hours of research to make the perfect setup for you.</p>
            <h2>What do we get out of it?</h2>
            <p>Honestly? Nothing. Experience. All components are free to use and no plans for ads or personal information gathering are in the works.</p>
            <h2>Do we gather information?</h2>
            <p>Yes, we gather information that is used in making the site a more effective and useful tool. No personaly identifying information will be released as is outlined in our {privacyPolicy}.</p>
        </div>
    );
}

export default About;