﻿using System;

namespace PoliteCaptcha
{
    /// <summary>
    /// The constants used by PoliteCaptcha, all in one convenience place.
    /// </summary>
    public static class Const
    {
        public const string DefaulFallbackMessage = "Your request failed spam prevention. You must complete the CAPTCHA form below to proceed.";
        public const string ModelStateKey = "PoliteCaptcha";
        public const string NoCaptchaChallengeField = "nocaptcha_challenge";
        public const string NoCaptchaResponseField = "nocaptcha_response";
        public const string ReCaptchaChallengeField = "recaptcha_challenge_field";
        public const string ReCaptchControlId = "RudeCaptcha";
        public const string ReCaptchaLocalhostPrivateKey = "6LehOM0SAAAAAC5LsEpHoyyMqJcz7f_zEfqm66um";
        public const string ReCaptchaLocalhostPublicKey = "6LehOM0SAAAAAPgsjOy-6_grqy1JiB_W_jJa_aCw";
        public const string ReCaptchaPrivateKeyAppSettingKey = "reCAPTCHA::PrivateKey";
        public const string ReCaptchaPublicKeyAppSettingKey = "reCAPTCHA::PublicKey";
        public const string ReCaptchaResponseField = "recaptcha_response_field";
    }
}
