import i18next from "i18next";
import { initReactI18next } from "react-i18next";

import translationEnglish from './Translation/English/translation.json';
import translationRussian from './Translation/Russian/translation.json';
import translationUkraine from './Translation/Ukranian/translation.json';

const resources = {
  en: {
    translation: translationEnglish,
  },
  ru: {
    translation: translationRussian,
  },
  ua: {
    translation: translationUkraine,
  }
}

i18next
.use(initReactI18next)
.init({
  resources,
  lng:"en"
});


export default i18next;