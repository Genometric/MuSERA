﻿/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System;

namespace Polimi.DEIB.VahidJalili.MuSERA.XSquaredData
{
    internal class DF14
    {
        internal double ChiSqrd(double x)
        {
            if (x < 1482.0)
                return Data[(int)Math.Round(x)];
            else
                return 0.0;
        }

        private readonly double[] Data = new double[]
        {
             #region .     Data     .
1   ,
9.99998997620397E-01    ,
9.99916758850712E-01    ,
9.99074008086475E-01    ,
9.95466194473751E-01    ,
9.85812688009087E-01    ,
9.66491464691159E-01    ,
9.34711902971046E-01    ,
8.89326021597426E-01    ,
8.31050578725411E-01    ,
7.62183462972939E-01    ,
6.86035980282304E-01    ,
6.06302782412591E-01    ,
5.26523622518000E-01    ,
4.49711055848699E-01    ,
3.78154694323469E-01    ,
3.13374277536398E-01    ,
2.56177861162932E-01    ,
2.06780839859987E-01    ,
1.64949244300816E-01    ,
1.30141420882483E-01    ,
1.01632500716557E-01    ,
7.86143720931333E-02    ,
6.02697228234131E-02    ,
4.58223068886511E-02    ,
3.45673935772488E-02    ,
2.58869154108351E-02    ,
1.92536200784674E-02    ,
1.42279183442616E-02    ,
1.04503579497337E-02    ,
7.63189963751496E-03    ,
5.54350937507792E-03    ,
4.00604465512781E-03    ,
2.88100566451687E-03    ,
2.06243078029543E-03    ,
1.47001977487620E-03    ,
1.04344551025760E-03    ,
7.37745888821163E-04    ,
5.19655828925500E-04    ,
3.64731107798754E-04    ,
2.55122495856301E-04    ,
1.77872971034605E-04    ,
1.23628463201411E-04    ,
8.56707229782755E-05    ,
5.91979121011428E-05    ,
4.07935571774571E-05    ,
2.80372990140428E-05    ,
1.92214228924752E-05    ,
1.31456602560523E-05    ,
8.96947681765463E-06    ,
6.10629446192790E-06    ,
4.14811037187755E-06    ,
2.81202295943712E-06    ,
1.90246067838980E-06    ,
1.28461022956663E-06    ,
8.65794753603971E-07    ,
5.82470555445999E-07    ,
3.91178209761697E-07    ,
2.62265948435007E-07    ,
1.75549443680524E-07    ,
1.17319420023470E-07    ,
7.82843238107941E-08    ,
5.21596547028930E-08    ,
3.47031777042570E-08    ,
2.30567009056340E-08    ,
1.52980614125298E-08    ,
1.01368751148772E-08    ,
6.70836477702961E-09    ,
4.43394033745721E-09    ,
2.92710761797723E-09    ,
1.93009041858404E-09    ,
1.27121966814773E-09    ,
8.36336105030747E-10    ,
5.49630931536404E-10    ,
3.60831004824144E-10    ,
2.36641388473063E-10    ,
1.55039764437361E-10    ,
1.01478016277337E-10    ,
6.63571679177047E-11    ,
4.33511741120248E-11    ,
2.82957240487240E-11    ,
1.84525805674096E-11    ,
1.20231662833877E-11    ,
7.82735112417455E-12    ,
5.09159051774658E-12    ,
3.30935222981745E-12    ,
2.14927157242528E-12    ,
1.39478096483136E-12    ,
9.04470762560373E-13    ,
5.86089583266748E-13    ,
3.79508349857478E-13    ,
2.45568820311983E-13    ,
1.58791096527711E-13    ,
1.02609219390223E-13    ,
6.62613093498352E-14    ,
4.27615000577606E-14    ,
2.75785134889238E-14    ,
1.77753975203799E-14    ,
1.14499532664831E-14    ,
7.37104815892908E-15    ,
4.74243067807484E-15    ,
3.04946645911861E-15    ,
1.95975885299743E-15    ,
1.25875716368288E-15    ,
8.08065066681282E-16    ,
5.18465844386552E-16    ,
3.32481645468397E-16    ,
2.13104711314657E-16    ,
1.36521267873443E-16    ,
8.74164942700087E-17    ,
5.59469080483315E-17    ,
3.57892127152439E-17    ,
2.28836434643365E-17    ,
1.46250951624331E-17    ,
9.34278092637412E-18    ,
5.96569321238570E-18    ,
3.80764293932758E-18    ,
2.42921104417570E-18    ,
1.54914158332405E-18    ,
9.87499390033713E-19    ,
6.29224133230850E-19    ,
4.00774053257212E-19    ,
2.55165716560465E-19    ,
1.62396360939301E-19    ,
1.03315191307928E-19    ,
6.57035160620446E-20    ,
4.17688123731163E-20    ,
2.65434412181984E-20    ,
1.68618936280301E-20    ,
1.07078398022803E-20    ,
6.79745142628113E-21    ,
4.31361482229621E-21    ,
2.73646462812111E-21    ,
1.73537651183310E-21    ,
1.10015814402621E-21    ,
6.97230100006788E-22    ,
4.41731915831473E-22    ,
2.79772524972736E-22    ,
1.77140116790563E-22    ,
1.12123405122654E-22    ,
7.09487886223602E-23    ,
4.48812426264102E-23    ,
2.83829630654512E-23    ,
1.79442501669012E-23    ,
1.13414697530561E-23    ,
7.16624223939927E-24    ,
4.52682180872664E-24    ,
2.85875342774702E-24    ,
1.80485768535261E-24    ,
1.13918365316578E-24    ,
7.18837230596354E-25    ,
4.53476626615457E-25    ,
2.86001453640539E-25    ,
1.80331666021099E-25    ,
1.13675684055077E-25    ,
7.16401296316219E-26    ,
4.51377293015121E-26    ,
2.84327546333248E-26    ,
1.79058669760639E-26    ,
1.12737976406111E-26    ,
7.09651027045389E-27    ,
4.46601714811936E-27    ,
2.80994678806065E-27    ,
1.76758029198384E-27    ,
1.11164140952136E-27    ,
6.98965816474190E-28    ,
4.39393811562812E-28    ,
2.76159392074483E-28    ,
1.73530040091019E-28    ,
1.09018335970655E-28    ,
6.84755469387448E-29    ,
4.30014974019086E-29    ,
2.69988189735854E-29    ,
1.69480629193439E-29    ,
1.06367868768534E-29    ,
6.67447171610816E-30    ,
4.18736028628811E-30    ,
2.62652587800402E-30    ,
1.64718307992995E-30    ,
1.03281323039470E-30    ,
6.47473990507122E-31    ,
4.05830183509601E-31    ,
2.54324792294823E-31    ,
1.59351527037865E-31    ,
9.98269412890200E-32    ,
6.25264996226532E-32    ,
3.91567002308178E-32    ,
2.45174027577945E-32    ,
1.53486441501685E-32    ,
9.60712667323696E-33    ,
6.01237017606244E-33    ,
3.76207406363438E-33    ,
2.35363510622657E-33    ,
1.47225081991062E-33    ,
9.20780390910545E-34    ,
5.75787986861361E-34    ,
3.59999669875428E-34    ,
2.25048045213490E-34    ,
1.40663911919767E-34    ,
8.79073311809216E-35    ,
5.49291782558991E-35    ,
3.43176346385782E-35    ,
2.14372194444959E-35    ,
1.33892743622234E-35    ,
8.36149078201221E-36    ,
5.22094449038224E-36    ,
3.25952046644495E-36    ,
2.03468979341436E-36    ,
1.26993979286241E-36    ,
7.92517850904052E-37    ,
4.94511650507309E-37    ,
3.08521976652522E-37    ,
1.92459045082456E-37    ,
1.20042139261233E-37    ,
7.48639660497799E-38    ,
4.66827207579498E-38    ,
2.91061139115547E-38    ,
1.81450233447186E-38    ,
1.13103638870275E-38    ,
7.04923283231942E-39    ,
4.39292561154266E-39    ,
2.73724100571542E-39    ,
1.70537499974713E-39    ,
1.06236775076403E-39    ,
6.61726393156935E-40    ,
4.12127011612274E-40    ,
2.56645229014921E-40    ,
1.59803116324816E-40    ,
9.94918858295813E-41    ,
6.19356758540122E-41    ,
3.85518588759313E-41    ,
2.39939312003938E-41    ,
1.49317101847675E-41    ,
9.29116472988271E-42    ,
5.78074266535920E-42    ,
3.59625418518339E-42    ,
2.23702472206551E-42    ,
1.39137832942222E-42    ,
8.65314771782722E-43    ,
5.38093579477061E-43    ,
3.34577464927605E-43    ,
2.08013305443708E-43    ,
1.29312783995417E-43    ,
8.03800155992410E-44    ,
4.99587247541758E-44    ,
3.10478539652917E-44    ,
1.92934174980983E-44    ,
1.19879359218309E-44    ,
7.44796586838428E-45    ,
4.62689124732865E-45    ,
2.87408485241800E-45    ,
1.78512604667061E-45    ,
1.10865780269808E-45    ,
6.88471232835227E-46    ,
4.27497957148487E-46    ,
2.65425452181454E-46    ,
1.64782722188668E-46    ,
1.02291999987748E-46    ,
6.34940248410036E-47    ,
3.94081033731395E-47    ,
2.44568203056841E-47    ,
1.51766711962290E-47    ,
9.41706176855793E-48    ,
5.84274535113953E-48    ,
3.62477809561497E-48    ,
2.24858389450852E-48    ,
1.39476244844110E-48    ,
8.65077762230447E-49    ,
5.36505366211226E-49    ,
3.32703429972032E-49    ,
2.06302758495956E-49    ,
1.27913858803453E-49    ,
7.93040253600162E-50    ,
4.91629781972369E-50    ,
3.04752100135937E-50    ,
1.88895256070925E-50    ,
1.17074270914216E-50    ,
7.25551397229546E-51    ,
4.49615686486620E-51    ,
2.78600259200432E-51    ,
1.72619002496759E-51    ,
1.06945606454778E-51    ,
6.62528830462686E-52    ,
4.10406597226577E-52    ,
2.54209530547611E-52    ,
1.57448124232712E-52    ,
9.75105355815955E-53    ,
6.03857132064392E-53    ,
3.73926016029430E-53    ,
2.31529430391499E-53    ,
1.43349431554020E-53    ,
8.87473119885961E-54    ,
5.49394249687415E-54    ,
3.40081404764907E-54    ,
2.10499825796328E-54    ,
1.30283937580353E-54    ,
8.06307112324274E-55    ,
4.98977293469250E-55    ,
3.08767760952592E-55    ,
1.91053140455621E-55    ,
1.18208218408909E-55    ,
7.31328690511866E-56    ,
4.52427700732325E-56    ,
2.79870798214146E-56    ,
1.73116312364017E-56    ,
1.07075617588345E-56    ,
6.62240220953957E-57    ,
4.09555789302859E-57    ,
2.53269743957829E-57    ,
1.56612512006645E-57    ,
9.68373008533509E-58    ,
5.98731550903237E-58    ,
3.70164726491517E-58    ,
2.28839772355627E-58    ,
1.41462633050230E-58    ,
8.74431690283046E-59    ,
5.40485596235035E-59    ,
3.34053947701801E-59    ,
2.06454094742004E-59    ,
1.27586569904828E-59    ,
7.88426384019136E-60    ,
4.87183105625443E-60    ,
3.01022063337709E-60    ,
1.85985732280879E-60    ,
1.14904298086632E-60    ,
7.09852987667960E-61    ,
4.38506666148043E-61    ,
2.70869295533944E-61    ,
1.67308997338488E-61    ,
1.03336774190035E-61    ,
6.38214597954794E-62    ,
3.94144017953461E-62    ,
2.43399487560846E-62    ,
1.50300710669406E-62    ,
9.28066725975530E-63    ,
5.73025965648537E-63    ,
3.53790746214097E-63    ,
2.18421728860023E-63    ,
1.34841181454218E-63    ,
8.32389759324623E-64    ,
5.13817049148734E-64    ,
3.17152417429390E-64    ,
1.95751637807518E-64    ,
1.20814976582195E-64    ,
7.45614357901165E-65    ,
4.60135769945405E-65    ,
2.83946224869128E-65    ,
1.75212342709330E-65    ,
1.08111504490358E-65    ,
6.67049195556560E-66    ,
4.11550068619005E-66    ,
2.53902204704995E-66    ,
1.56635199404010E-66    ,
9.66254374719254E-67    ,
5.96036582015416E-67    ,
3.67649354091608E-67    ,
2.26764080514624E-67    ,
1.39860281345744E-67    ,
8.62569947528463E-68    ,
5.31954089192225E-68    ,
3.28045432168221E-68    ,
2.02289789566013E-68    ,
1.24736675306032E-68    ,
7.69121068887380E-69    ,
4.74215453261357E-69    ,
2.92372992322618E-69    ,
1.80251739789053E-69    ,
1.11122612975495E-69    ,
6.85024802243068E-70    ,
4.22270868433884E-70    ,
2.60289724437601E-70    ,
1.60436841764556E-70    ,
9.88854658603960E-71    ,
6.09455783374269E-71    ,
3.75606776933417E-71    ,
2.31476129998528E-71    ,
1.42646355528649E-71    ,
8.79016280210375E-72    ,
5.41645356030596E-72    ,
3.33745248456955E-72    ,
2.05635085215519E-72    ,
1.26695587591486E-72    ,
7.80563084463688E-73    ,
4.80880162903661E-73    ,
2.96243031643117E-73    ,
1.82491206826186E-73    ,
1.12413469371609E-73    ,
6.92432522398482E-74    ,
4.26500309728974E-74    ,
2.62690349889261E-74    ,
1.61790064988161E-74    ,
9.96420442981380E-75    ,
6.13644073338316E-75    ,
3.77897205029161E-75    ,
2.32709504353562E-75    ,
1.43297281763540E-75    ,
8.82358871633270E-76    ,
5.43295511713214E-76    ,
3.34511163868058E-76    ,
2.05953338204988E-76    ,
1.26797548256700E-76    ,
7.80614759357334E-77    ,
4.80558897042884E-77    ,
2.95828851042364E-77    ,
1.82103608170908E-77    ,
1.12093588507820E-77    ,
6.89965331760800E-78    ,
4.24676391603244E-78    ,
2.61380627407705E-78    ,
1.60869302183253E-78    ,
9.90050937662254E-79    ,
6.09293514324548E-79    ,
3.74955993222790E-79    ,
2.30737852500788E-79    ,
1.41984935424998E-79    ,
8.73676467599975E-80    ,
5.37581145208831E-80    ,
3.30767255429558E-80    ,
2.03510187240319E-80    ,
1.25208851302921E-80    ,
7.70316520222913E-81    ,
4.73902244089294E-81    ,
2.91537053706312E-81    ,
1.79342932208797E-81    ,
1.10321548781553E-81    ,
6.78612852061145E-82    ,
4.17416414976327E-82    ,
2.56745444804847E-82    ,
1.57914429768245E-82    ,
9.71240530640587E-83    ,
5.97334715891865E-83    ,
3.67362444226102E-83    ,
2.25921655803073E-83    ,
1.38933551969977E-83    ,
8.54363427496766E-84    ,
5.25369016087503E-84    ,
3.23052140489917E-84    ,
1.98640230736631E-84    ,
1.22137290801989E-84    ,
7.50958432846136E-85    ,
4.61710892405774E-85    ,
2.83864450968167E-85    ,
1.74517355073071E-85    ,
1.07288462698226E-85    ,
6.59559937029218E-86    ,
4.05454773153398E-86    ,
2.49239877836357E-86    ,
1.53207371894355E-86    ,
9.41735353317101E-87    ,
5.78848912765522E-87    ,
3.55785924176881E-87    ,
2.18675209896800E-87    ,
1.34399498290809E-87    ,
8.26005814519664E-88    ,
5.07640167110056E-88    ,
3.11972508085855E-88    ,
1.91718577193612E-88    ,
1.17814746028806E-88    ,
7.23973618330788E-89    ,
4.44870408894480E-89    ,
2.73358158216015E-89    ,
1.67964830332591E-89    ,
1.03203046925110E-89    ,
6.34095372031922E-90    ,
3.89587128409510E-90    ,
2.39355063819517E-90    ,
1.47051241407151E-90    ,
9.03405804405533E-91    ,
5.54990072659181E-91    ,
3.40938275480286E-91    ,
2.09437556588724E-91    ,
1.28653519592931E-91    ,
7.90272986996392E-92    ,
4.85423735398073E-92    ,
2.98162713786795E-92    ,
1.83136183647180E-92    ,
1.12482131499314E-92    ,
6.90846423649384E-93    ,
4.24295216810449E-93    ,
2.60581422956366E-93    ,
1.60032248377209E-93    ,
9.82789141284901E-94    ,
6.03534394677136E-94    ,
3.70623181683386E-94    ,
2.27589415308625E-94    ,
1.39752797712622E-94    ,
8.58139666800704E-95    ,
5.26919783494910E-95    ,
3.23534156748406E-95    ,
1.98648330899545E-95    ,
1.21966032644824E-95    ,
7.48828063499266E-96    ,
4.59742452412414E-96    ,
2.82251610915830E-96    ,
1.73279672456077E-96    ,
1.06377117526089E-96    ,
6.53037726177956E-97    ,
4.00883166427551E-97    ,
2.46085999667593E-97    ,
1.51058640708706E-97    ,
9.27243631784045E-98    ,
5.69156642205873E-98    ,
3.49348965167817E-98    ,
2.14425710557475E-98    ,
1.31608530336365E-98    ,
8.07757587187959E-99    ,
4.95755929997223E-99    ,
3.04259883388167E-99    ,
1.86728847126050E-99    ,
1.14595647079448E-99    ,
7.03258272968930E-100   ,
4.31570390322131E-100   ,
2.64836922082590E-100   ,
1.62515789727326E-100   ,
9.97247074175389E-101   ,
6.11927815250275E-101   ,
3.75480918563351E-101   ,
2.30391174989384E-101   ,
1.41362473597781E-101   ,
8.67346834339993E-102   ,
5.32159551951239E-102   ,
3.26498631405248E-102   ,
2.00314018808042E-102   ,
1.22894326815321E-102   ,
7.53950570017013E-103   ,
4.62534888605487E-103   ,
2.83750567824516E-103   ,
1.74068279575230E-103   ,
1.06780813461914E-103   ,
6.55024491416163E-104   ,
4.01802479650672E-104   ,
2.46466831336193E-104   ,
1.51180292185847E-104   ,
9.27305302061895E-105   ,
5.68775922598273E-105   ,
3.48859533835873E-105   ,
2.13969047981756E-105   ,
1.31232774838422E-105   ,
8.04868075691803E-106   ,
4.93626149030955E-106   ,
3.02735054664510E-106   ,
1.85660022322589E-106   ,
1.13858445107968E-106   ,
6.98237702513161E-107   ,
4.28186106422733E-107   ,
2.62574839229447E-107   ,
1.61014481554085E-107   ,
9.87343093026942E-108   ,
6.05428140213407E-108   ,
3.71234646600629E-108   ,
2.27628071183475E-108   ,
1.39570805650135E-108   ,
8.55765616629499E-109   ,
5.24694597494209E-109   ,
3.21699135558022E-109   ,
1.97235355747171E-109   ,
1.20923649080049E-109   ,
7.41360347498217E-110   ,
4.54505481699107E-110   ,
2.78638157626717E-110   ,
1.70818056454146E-110   ,
1.04717367030187E-110   ,
6.41941469979378E-111   ,
3.93517418558404E-111   ,
2.41226117851084E-111   ,
1.47868804670363E-111   ,
9.06401661512273E-112   ,
5.55592978384236E-112   ,
3.40553036561191E-112   ,
2.08739551940550E-112   ,
1.27943063727425E-112   ,
7.84189065972376E-113   ,
4.80636650080111E-113   ,
2.94581234737484E-113   ,
1.80544969566667E-113   ,
1.10651639732093E-113   ,
6.78144884964361E-114   ,
4.15603626336153E-114   ,
2.54699677395106E-114   ,
1.56088072199997E-114   ,
9.56540407255356E-115   ,
5.86177619796912E-115   ,
3.59209224091095E-115   ,
2.20119282561540E-115   ,
1.34884201125135E-115   ,
8.26525948538126E-116   ,
5.06459054846964E-116   ,
3.10330636594173E-116   ,
1.90150492513561E-116   ,
1.16509884723997E-116   ,
7.13872465737869E-117   ,
4.37392222519960E-117   ,
2.67987213177054E-117   ,
1.64191134852132E-117   ,
1.00595366589123E-117   ,
6.16309576660443E-118   ,
3.77583097406721E-118   ,
2.31323038292767E-118   ,
1.41715701130858E-118   ,
8.68180075447864E-119   ,
5.31856469739048E-119   ,
3.25815611272530E-119   ,
1.99591557482558E-119   ,
1.22265887123254E-119   ,
7.48964652471646E-120   ,
4.58786093584247E-120   ,
2.81029621263946E-120   ,
1.72142023199625E-120   ,
1.05442254480071E-120   ,
6.45855647465240E-121   ,
3.95593603790106E-121   ,
2.42301501027239E-121   ,
1.48407556538322E-121   ,
9.08968878537685E-122   ,
5.56717822698625E-122   ,
3.40968587073593E-122   ,
2.08827058487290E-122   ,
1.27894642008992E-122   ,
7.83269346280622E-123   ,
4.79692725697066E-123   ,
2.93770632894134E-123   ,
1.79906515032837E-123   ,
1.10173891685561E-123   ,
6.74689262659994E-124   ,
4.13163775340615E-124   ,
2.53007868180107E-124   ,
1.54931308993390E-124   ,
9.48719324622383E-125   ,
5.80937910598816E-125   ,
3.55725604144727E-125   ,
2.17818111991409E-125   ,
1.33372511775390E-125   ,
8.16642859139695E-126   ,
5.00024778101196E-126   ,
3.06157160534489E-126   ,
1.87452344192340E-126   ,
1.14770668048834E-126   ,
7.02691244864381E-127   ,
4.30221187559048E-127   ,
2.63398129284407E-127   ,
1.61260215800829E-127   ,
9.87268921319791E-128   ,
6.04418005659019E-128   ,
3.70026680037994E-128   ,
2.26528277242020E-128   ,
1.38677358914005E-128   ,
8.48950713966727E-129   ,
5.19700565131965E-129   ,
3.18139588231833E-129   ,
1.94749375439100E-129   ,
1.19214281271259E-129   ,
7.29750442864685E-130   ,
4.46698340995272E-130   ,
2.73431295692367E-130   ,
1.67369388149738E-130   ,
1.02446654868903E-130   ,
6.27066355710532E-131   ,
3.83816110584573E-131   ,
2.34923737189444E-131   ,
1.43788654781760E-131   ,
8.80068298397769E-132   ,
5.38644449102976E-132   ,
3.29672007666973E-132   ,
2.01769746249836E-132   ,
1.23487822068512E-132   ,
7.55764242907804E-133   ,
4.62532968166360E-133   ,
2.83069584767240E-133   ,
1.73235901625684E-133   ,
1.06017312211597E-133   ,
6.48798581711448E-134   ,
3.97042708209135E-134   ,
2.42973441319607E-134   ,
1.48687565182154E-134   ,
9.09881377413947E-135   ,
5.56787147272941E-135   ,
3.40712401912106E-135   ,
2.08487976782760E-135   ,
1.27575839955004E-135   ,
7.80638962103961E-136   ,
4.77668268734219E-136   ,
2.92278560420478E-136   ,
1.78838880160342E-136   ,
1.09426209358352E-136   ,
6.69537976934161E-137   ,
4.09659980425487E-137   ,
2.50649212096499E-137   ,
1.53357013810370E-137   ,
9.38286471321924E-138   ,
5.74065956916398E-138   ,
3.51222779379199E-138   ,
2.14881049550822E-138   ,
1.31464390606422E-138   ,
8.04290129806442E-139   ,
4.92053076564877E-139   ,
3.01027232230889E-139   ,
1.84159554798838E-139   ,
1.12661979984119E-139   ,
6.89215713077725E-140   ,
4.21626251750947E-140   ,
2.57925817010608E-140   ,
1.57781724792504E-140   ,
9.65191106666979E-141   ,
5.90424903166090E-141   ,
3.61169260833139E-141   ,
2.20928460572252E-141   ,
1.35141065889727E-141   ,
8.26642663670339E-142   ,
5.05641962794205E-142   ,
3.09288115682392E-142   ,
1.89181298844895E-142   ,
1.15714566706623E-142   ,
7.07770959201186E-143   ,
4.32904732664222E-143   ,
2.64781008460728E-143   ,
1.61948245687129E-143   ,
9.90513948483990E-144   ,
6.05814814789521E-144   ,
3.70522122640538E-144   ,
2.26612243944975E-144   ,
1.38595007755492E-144   ,
8.47630947725914E-145   ,
5.18395279400709E-145   ,
3.17037256282391E-145   ,
1.93889641078195E-145   ,
1.18575228241100E-145   ,
7.25150919853723E-146   ,
4.43463539516381E-146   ,
2.71195533866168E-146   ,
1.65844952419061E-146   ,
1.01418482289578E-146   ,
6.20193333421870E-147   ,
3.79255799638099E-147   ,
2.31916971918294E-147   ,
1.41816907263093E-147   ,
8.67198820248944E-148   ,
5.30279148053239E-148   ,
3.24254220480565E-148   ,
1.98272258832107E-148   ,
1.21236534338475E-148   ,
7.41310799104435E-149   ,
4.53275669498134E-149   ,
2.77153118521317E-149   ,
1.69462037682570E-149   ,
1.03614463946776E-149   ,
6.33524862012742E-150   ,
3.87348851143321E-150   ,
2.36829732617493E-150   ,
1.44798990057231E-150   ,
8.85299479493165E-151   ,
5.41265399165785E-151   ,
3.30922056404878E-151   ,
2.02318952776541E-151   ,
1.23692349635024E-151   ,
7.56213697368913E-152   ,
4.62318934880446E-152   ,
2.82640423770194E-152   ,
1.72791484389668E-152   ,
1.05634522294792E-152   ,
6.45780458371365E-153   ,
3.94783893703611E-153   ,
2.41340087618366E-153   ,
1.47534989638370E-153   ,
9.01895315487163E-154   ,
5.51331466170810E-154   ,
3.37027227475455E-154   ,
2.06021605275308E-154   ,
1.25937807555326E-154   ,
7.69830448004948E-155   ,
4.70575860785785E-155   ,
2.87646983100548E-155   ,
1.75827025750732E-155   ,
1.07474906408319E-155   ,
6.56937679726714E-156   ,
4.01547504195390E-156   ,
2.45440011576316E-156   ,
1.50020109772390E-156   ,
9.16957676106484E-157   ,
5.60460240020958E-157   ,
3.42559521953916E-157   ,
2.09374124183871E-157   ,
1.27969275406745E-157   ,
7.82139400622749E-158   ,
4.78033550726976E-158   ,
2.92165118742374E-158   ,
1.78564093564013E-158   ,
1.09132899912815E-158   ,
6.66980384166702E-159   ,
4.07630096849592E-159   ,
2.49123816090727E-159   ,
1.52250977369622E-159   ,
9.30466575530943E-160   ,
5.68639888864778E-160   ,
3.47511941442939E-160   ,
2.12372373925128E-160   ,
1.29784311785862E-160   ,
7.93126157938553E-161   ,
4.84683524076860E-161   ,
2.96189841307810E-161   ,
1.80999756260377E-161   ,
1.10606782746969E-161   ,
6.75898494699911E-162   ,
4.13025777703141E-162   ,
2.52388059937537E-162   ,
1.54225587132342E-162   ,
9.42410353002800E-163   ,
5.75863647718005E-163   ,
3.51880571273977E-163   ,
2.15014095633007E-163   ,
1.31381617387013E-163   ,
8.02783272811919E-164   ,
4.90521529511973E-164   ,
2.99718734935466E-164   ,
1.83132648203219E-164   ,
1.11895787650246E-164   ,
6.83687731501602E-165   ,
4.17732179863400E-165   ,
2.55231447731555E-165   ,
1.55943239226402E-165   ,
9.52785289529181E-166   ,
5.82129582784649E-166   ,
3.55664435615590E-166   ,
2.17298818088227E-166   ,
1.32760980262443E-166   ,
8.11109911855112E-167   ,
4.95547338319409E-167   ,
3.02751830305672E-167   ,
1.84962891683637E-167   ,
1.13000052251756E-167   ,
6.90349317676870E-168   ,
4.21750282830231E-168   ,
2.57654718993390E-168   ,
1.57404471178146E-168   ,
9.61595189838327E-169   ,
5.87440334873556E-169   ,
3.58865340007498E-169   ,
2.19227761091463E-169   ,
1.33923216535010E-169   ,
8.18111491702178E-170   ,
4.99764521269084E-170   ,
3.05291467778443E-170   ,
1.86492012980627E-170   ,
1.13920567593545E-170   ,
6.95889663606504E-171   ,
4.25084218956001E-171   ,
2.59660529493614E-171   ,
1.58610985135441E-171   ,
9.68850936512109E-172   ,
5.91802846120183E-172   ,
3.61487703879352E-172   ,
2.20803732829885E-172   ,
1.34870107509126E-172   ,
8.23799293686712E-173   ,
5.03180212545804E-173   ,
3.07342152778338E-173   ,
1.87722853776901E-173   ,
1.14659123872690E-173   ,
7.00320034642893E-174   ,
4.27741069940653E-174   ,
2.61253326634855E-174   ,
1.59565571562670E-174   ,
9.74570022796183E-175   ,
5.95228073963345E-175   ,
3.63538385454136E-175   ,
2.22031022694831E-175   ,
1.35604334065703E-175   ,
8.28190062308775E-176   ,
5.05804864009711E-176   ,
3.08910405411323E-176   ,
1.88659479136502E-176   ,
1.15218254134702E-176   ,
7.03656206604283E-177   ,
4.29730656063923E-177   ,
2.62439220512622E-177   ,
1.60272030366009E-177   ,
9.78776070138411E-178   ,
5.97730696050529E-178   ,
3.65026501266686E-178   ,
2.22915290905070E-178   ,
1.36129409163319E-178   ,
8.31305592483425E-179   ,
5.07651992815771E-179   ,
3.10004606153575E-179   ,
1.89307083160880E-179   ,
1.15601176596186E-179   ,
7.05918113183363E-180   ,
4.31065320651628E-180   ,
2.63225852170547E-180   ,
1.60735090369040E-180   ,
9.81498336037573E-181   ,
5.99328809460179E-181   ,
3.65963242353262E-181   ,
2.23463456183213E-181   ,
1.36449609202600E-181   ,
8.33172310162798E-182   ,
5.08737925178691E-182   ,
3.10634839302633E-182   ,
1.89671893348988E-182   ,
1.15811736218704E-182   ,
7.07129489041654E-183   ,
4.31759712065336E-183   ,
2.63622260438295E-183   ,
1.60960327980370E-183   ,
9.82771217308368E-184   ,
6.00043627336606E-184   ,
3.66361688989854E-184   ,
2.23683582623608E-184   ,
1.36569904944172E-184   ,
8.33820850518059E-185   ,
5.09081538820734E-185   ,
3.10812735729291E-185   ,
1.89761074593973E-185   ,
1.15854346099468E-185   ,
7.07317512019092E-186   ,
4.31830565292556E-186   ,
2.63638748611379E-186   ,
1.60954085816882E-186   ,
9.82633753389839E-187   ,
5.99899175742049E-187   ,
3.66236625678396E-187   ,
2.23584766781666E-187   ,
1.36495892604069E-187   ,
8.33285637462162E-188   ,
5.08704006393643E-188   ,
3.10551316318445E-188   ,
1.89582633657633E-188   ,
1.15733929188012E-188   ,
7.06512447543604E-189   ,
4.31296485007148E-189   ,
2.63286752105227E-189   ,
1.60723391968013E-189   ,
9.81129133848844E-190   ,
5.98921993240203E-190   ,
3.65604357903788E-190   ,
2.23177025906596E-190   ,
1.36233725685034E-190   ,
8.31604467893162E-191   ,
5.07628541921195E-191   ,
3.09864837337683E-191   ,
1.89145324872280E-191   ,
1.15455860783153E-191   ,
7.04747297988855E-192   ,
4.30177731762867E-192   ,
2.62578708089966E-192   ,
1.60275880510574E-192   ,
9.78304213766570E-193   ,
5.97140835442644E-193   ,
3.64482532011578E-193   ,
2.22471188134519E-193   ,
1.35790048037901E-193   ,
8.28818103648934E-194   ,
5.05880152070879E-194   ,
3.08768638827810E-194   ,
1.88458557732252E-194   ,
1.15025912210244E-194   ,
7.02057459401599E-195   ,
4.28496012784234E-195   ,
2.61527927991522E-195   ,
1.59619713809205E-195   ,
9.74209040243758E-196   ,
5.94586386474875E-196   ,
3.62889959389626E-196   ,
2.21478785356895E-196   ,
1.35171928585290E-196   ,
8.24969873785122E-197   ,
5.03485393834198E-197   ,
3.07278996969652E-197   ,
1.87532306951307E-197   ,
1.14450196027365E-197   ,
6.98480387703041E-198   ,
4.26274278626697E-198   ,
2.60148473626979E-198   ,
1.58763507066744E-198   ,
9.68896392829130E-199   ,
5.91290979055107E-199   ,
3.60846445975947E-199   ,
2.20211949381928E-199   ,
1.34386798080434E-199   ,
8.20105289428019E-200   ,
5.00472139973962E-200   ,
3.05412981247329E-200   ,
1.86377025481384E-200   ,
1.13735113059172E-200   ,
6.94055276168821E-201   ,
4.23536526794246E-201   ,
2.58455037631328E-201   ,
1.57716255520669E-201   ,
9.62421340260009E-202   ,
5.87288324627584E-202   ,
3.58372627962669E-202   ,
2.18683311913364E-202   ,
1.33442388217521E-202   ,
8.14271673109304E-203   ,
4.96869353389204E-203   ,
3.03188317101371E-203   ,
1.85003560910476E-203   ,
1.12887301510424E-203   ,
6.88822745704691E-204   ,
4.20307613229539E-204   ,
2.56462828726678E-204   ,
1.56487264617753E-204   ,
9.54840815517360E-205   ,
5.82613254755939E-205   ,
3.55489814421612E-205   ,
2.16905908783712E-205   ,
1.32346673356658E-205   ,
8.07517804166783E-206   ,
4.92706871350818E-206   ,
3.00623254645750E-206   ,
1.83423075585032E-206   ,
1.11913588366816E-206   ,
6.82824549168823E-207   ,
4.16613072428445E-207   ,
2.54187462285977E-207   ,
1.55086083438818E-207   ,
9.46213210828413E-208   ,
5.77301474758632E-208   ,
3.52219837442424E-208   ,
2.14893088796914E-208   ,
1.31107815076643E-208   ,
7.99893581492560E-209   ,
4.88015200377343E-209   ,
2.97736443910441E-209   ,
1.81646970734277E-209   ,
1.10820943249855E-209   ,
6.76103290739769E-210   ,
4.12478946778390E-210   ,
2.51644856551325E-210   ,
1.53522441589352E-210   ,
9.36597993912519E-211   ,
5.71389330362896E-211   ,
3.48584910248121E-211   ,
2.12658427458920E-211   ,
1.29734109722623E-211   ,
7.91449704628141E-212   ,
4.82825322350151E-212   ,
2.94546816968436E-212   ,
1.79686814811015E-212   ,
1.09616434853936E-212   ,
6.68702161097649E-213   ,
4.07931625580129E-213   ,
2.48851134781040E-213   ,
1.51806189720119E-213   ,
9.26055346448326E-214   ,
5.64913587961740E-214   ,
3.44607493637145E-214   ,
2.10215645804885E-214   ,
1.28233939072783E-214   ,
7.82237373947771E-215   ,
4.77168512309009E-215   ,
2.91073477210031E-215   ,
1.77554276205519E-215   ,
1.08307190058977E-215   ,
6.60664688973828E-216   ,
4.02997694082221E-216   ,
2.45822533521667E-216   ,
1.49947243793971E-216   ,
9.14645825454461E-217   ,
5.57911228884309E-217   ,
3.40310170995148E-217   ,
2.07578534466517E-217   ,
1.26615724209441E-217   ,
7.72308010434288E-218   ,
4.71076168226666E-218   ,
2.87335595940536E-218   ,
1.75261060436499E-218   ,
1.06900355779870E-218   ,
6.52034509429265E-219   ,
3.97703792740736E-219   ,
2.42575317129359E-219   ,
1.47955533172134E-219   ,
9.02430048011635E-220   ,
5.50419257930018E-220   ,
3.35715532023044E-220   ,
2.04760883065518E-220   ,
1.24887882644224E-220   ,
7.61712995336475E-221   ,
4.64579652929833E-221   ,
2.83352316398311E-221   ,
1.72818851874859E-221   ,
1.05403063584763E-221   ,
6.42855149045752E-222   ,
3.92076486809155E-222   ,
2.39125698600820E-222   ,
1.45840952553872E-222   ,
8.89468399517546E-223   ,
5.42474526273623E-223   ,
3.30846065240367E-223   ,
2.01776414965360E-223   ,
1.23058788715119E-223   ,
7.50503429803507E-224   ,
4.57710148216806E-224   ,
2.79142665219120E-224   ,
1.70239260013549E-224   ,
1.03822397088533E-224   ,
6.33169828057659E-225   ,
3.86142146268973E-225   ,
2.35489766715663E-225   ,
1.43613317768472E-225   ,
8.75820765453478E-226   ,
5.34113568719850E-226   ,
3.25724059246196E-226   ,
1.98638727367460E-226   ,
1.21136737244740E-226   ,
7.38729914419539E-227   ,
4.50498521118111E-227   ,
2.74725471309652E-227   ,
1.67533770257732E-227   ,
1.02165362104213E-227   ,
6.23021279309933E-228   ,
3.79926836025276E-228   ,
2.31683419441239E-228   ,
1.41282325387349E-228   ,
8.61546286454406E-229   ,
5.25372455171974E-229   ,
3.20371512649786E-229   ,
1.95361236695179E-229   ,
1.19129910423827E-229   ,
7.26442348408749E-230   ,
4.42975202153224E-230   ,
2.70119292036632E-230   ,
1.64713699176459E-230   ,
1.00438859515014E-230   ,
6.12451583805538E-231   ,
3.73456216217862E-231   ,
2.27722303505076E-231   ,
1.38857516096717E-231   ,
8.46703136306017E-232   ,
5.16286656079667E-232   ,
3.14810052523525E-232   ,
1.91957129173522E-232   ,
1.17046347862030E-232   ,
7.13689748147533E-233   ,
4.35170075356489E-233   ,
2.65342346590054E-233   ,
1.61790154127015E-233   ,
9.86496608115836E-234   ,
6.01502022497152E-234   ,
3.66755452433304E-234   ,
2.23621760001452E-234   ,
1.36348241747700E-234   ,
8.31348322353437E-235   ,
5.06890921542697E-235   ,
3.09060861177975E-235   ,
1.88439316479772E-235   ,
1.14893919728595E-235   ,
7.00520084504407E-236   ,
4.27112379775177E-236   ,
2.60412456336156E-236   ,
1.58773997137912E-236   ,
9.68043862238029E-237   ,
5.90212943886230E-237   ,
3.59849135546132E-237   ,
2.19396775864164E-237   ,
1.33763635979917E-237   ,
8.15537507677605E-238   ,
4.97219173675684E-238   ,
3.03144611014136E-238   ,
1.84820396314020E-238   ,
1.12680302889768E-238   ,
6.86980138432258E-239   ,
4.18830622083343E-239   ,
2.55346991940291E-239   ,
1.55675812914967E-239   ,
9.49094853634947E-240   ,
5.78623646912290E-240   ,
3.52761210872181E-240   ,
2.15061941009696E-240   ,
1.31112588297909E-240   ,
7.99324854296956E-241   ,
4.87304411774224E-241   ,
2.97081407170038E-241   ,
1.81112617715520E-241   ,
1.10412959935603E-241   ,
6.73115374151411E-242   ,
4.10352499905944E-242   ,
2.50162827010142E-242   ,
1.52505880816555E-242   ,
9.29712202835552E-243   ,
5.66772278552173E-243   ,
3.45514916275464E-243   ,
2.10631410930640E-243   ,
1.28403721465247E-243   ,
7.82762886561984E-244   ,
4.77178629771994E-244   ,
2.90890737648571E-244   ,
1.77327850932183E-244   ,
1.08099120977832E-244   ,
6.58969829197867E-245   ,
4.01704835406286E-245   ,
2.44876297984653E-245   ,
1.49274150630036E-245   ,
9.09956508500154E-246   ,
5.54695745494372E-246   ,
3.38132728840311E-246   ,
2.06118874500739E-246   ,
1.25645372069606E-246   ,
7.65902373844796E-247   ,
4.66872745437201E-247   ,
2.84591430587490E-247   ,
1.73477561635358E-247   ,
1.05745768091516E-247   ,
6.44586020554894E-248   ,
3.92913518658175E-248   ,
2.39503169975607E-248   ,
1.45990221968985E-248   ,
8.89886223168186E-249   ,
5.42429639211566E-249   ,
3.30636319692996E-249   ,
2.01537526737289E-249   ,
1.22845574103124E-249   ,
7.48792231567948E-250   ,
4.56416540723950E-250   ,
2.78201618313844E-250   ,
1.69572789260902E-250   ,
1.03359622265955E-250   ,
6.30004866045616E-251   ,
3.84003460298473E-251   ,
2.34058608252933E-251   ,
1.42663327102566E-251   ,
8.69557549876187E-252   ,
5.30008173724483E-252   ,
3.23046516540626E-252   ,
1.96900046255582E-252   ,
1.20012045395814E-252   ,
7.31479439580181E-253   ,
4.45838612669616E-253   ,
2.71738707810565E-253   ,
1.65624129248679E-253   ,
1.00947132725821E-253   ,
6.15265620134338E-254   ,
3.74998552939574E-254   ,
2.28557155054733E-254   ,
1.39302317021878E-254   ,
8.49024358452460E-255   ,
5.17464135325996E-255   ,
3.15383273479859E-255   ,
1.92218577142530E-255   ,
1.17152176734871E-255   ,
7.14008976857535E-256   ,
4.35166334215443E-256   ,
2.65219357213440E-256   ,
1.61641718946864E-256   ,
9.85144684795256E-257   ,
6.00405823263775E-257   ,
3.65921640807293E-257   ,
2.23012711396499E-257   ,
1.35915650543954E-257   ,
8.28338120270161E-258   ,
5.04828843521583E-258   ,
3.07665647621301E-258   ,
1.87504714971423E-258   ,
1.14273023500165E-258   ,
6.96423771491846E-259   ,
4.24425824314822E-259   ,
2.58659457951439E-259   ,
1.57635226945056E-259   ,
9.60675119505498E-260   ,
5.85461263846047E-260   ,
3.56794497066095E-260   ,
2.17438523549585E-260   ,
1.32511386252254E-260   ,
8.07547860231229E-261   ,
4.92132122436504E-261   ,
2.99911782071158E-261   ,
1.82769496678013E-261   ,
1.11381299645138E-261   ,
6.78764664924731E-262   ,
4.13641926693152E-262   ,
2.52074122141135E-262   ,
1.53613845597993E-262   ,
9.36118545465494E-263   ,
5.70465952021967E-263   ,
3.47637808290560E-263   ,
2.11847173859583E-263   ,
1.29097177072628E-263   ,
7.86700124752298E-264   ,
4.79402281938850E-264   ,
2.92138894812607E-264   ,
1.78023394018843E-264   ,
1.08483373852447E-264   ,
6.61070389384946E-265   ,
4.02838196624656E-265   ,
2.45477674848326E-265   ,
1.49586286504278E-265   ,
9.11527940222826E-266   ,
5.55452104308932E-266   ,
3.38471165546895E-266   ,
2.06250575577158E-266   ,
1.25680267284519E-266   ,
7.65838964533240E-267   ,
4.66666107735844E-267   ,
2.84363273033010E-267   ,
1.73276310334789E-267   ,
1.05585267695810E-267   ,
6.43377557501875E-268   ,
3.92036895097958E-268   ,
2.38883650833369E-268   ,
1.45560778706065E-268   ,
8.86953334937071E-269   ,
5.40450138269385E-269   ,
3.29313061554670E-269   ,
2.00659971377935E-269   ,
1.22267491770932E-269   ,
7.45005930908305E-270   ,
4.53948859710517E-270   ,
2.76600272451042E-270   ,
1.68537580347999E-270   ,
1.02692655642056E-270   ,
6.25720663082016E-271   ,
3.81258989753474E-271   ,
2.32304795405173E-271   ,
1.41545069380764E-271   ,
8.62441819637646E-272   ,
5.25488676347589E-272   ,
3.20180893409639E-272   ,
1.95085935255613E-272   ,
1.18865277313958E-272   ,
7.24240084604337E-273   ,
4.41274277782962E-273   ,
2.68864321207118E-273   ,
1.63815972726095E-273   ,
9.98108667312175E-274   ,
6.08132092061532E-274   ,
3.70524161992032E-274   ,
2.25753069015945E-274   ,
1.37546426801318E-274   ,
8.38037562235109E-275   ,
5.10594558045666E-275   ,
3.11090970362424E-275   ,
1.89538377480064E-275   ,
1.15479645748616E-275   ,
7.03578015762785E-276   ,
4.28664594600019E-276   ,
2.61168927892485E-276   ,
1.59119695155223E-276   ,
9.69448877772872E-277   ,
5.90642142675365E-277   ,
3.59850819668560E-277   ,
2.19239655242064E-277   ,
1.33571645348190E-277   ,
8.13781839966596E-278   ,
4.95792859634597E-278   ,
3.02058526163114E-278   ,
1.84026552322105E-278   ,
1.12116218792887E-278   ,
6.83053874119569E-279   ,
4.16140554378898E-279   ,
2.53526693307243E-279   ,
1.54456401671867E-279   ,
9.40993679375625E-280   ,
5.73279053916991E-280   ,
3.49256114808608E-280   ,
2.12774971806440E-280   ,
1.29627052363950E-280   ,
7.89713082002395E-281   ,
4.81106920624657E-281   ,
2.93097735499714E-281   ,
1.78559068257721E-281   ,
1.08780224379532E-281   ,
6.62699408278255E-282   ,
4.03721437257255E-282   ,
2.45949325552491E-282   ,
1.49833202013779E-282   ,
9.12786245042468E-283   ,
5.56069041398458E-283   ,
3.38755965804743E-283   ,
2.06368684313843E-283   ,
1.25718516649929E-283   ,
7.65866921991536E-284   ,
4.66558376252824E-284   ,
2.84221734078460E-284   ,
1.73143900376511E-284   ,
1.05476504321761E-284   ,
6.42544013055497E-285   ,
3.91425088528659E-285   ,
2.38447658078735E-285   ,
1.45256672759686E-285   ,
8.84866497784983E-286   ,
5.39036339759708E-286   ,
3.28365083575756E-286   ,
2.00029722382895E-286   ,
1.21851458413005E-286   ,
7.42276259380405E-287   ,
4.52167195276200E-287   ,
2.75442641913915E-287   ,
1.67788404731333E-287   ,
1.02209523153010E-287   ,
6.22614783927856E-288   ,
3.79267952172401E-288   ,
2.31031670331092E-288   ,
1.40732870039044E-288   ,
8.57271188938420E-289   ,
5.22203250817734E-289   ,
3.18097001195428E-289   ,
1.93766297876095E-289   ,
1.18030860480411E-289   ,
7.18971328395590E-290   ,
4.37951722398247E-290   ,
2.66771589418401E-290   ,
1.62499334379961E-290   ,
9.89833779891979E-291   ,
6.02936577656409E-291   ,
3.67265108115834E-291   ,
2.23710510649643E-291   ,
1.36267343604332E-291   ,
8.30033984626481E-292   ,
5.05590196687032E-292   ,
3.07964106524641E-292   ,
1.87585924943409E-292   ,
1.14261280609906E-292   ,
6.95979773641053E-293   ,
4.23928724688776E-293   ,
2.58218745903325E-293   ,
1.57282856882706E-293   ,
9.58018092699525E-294   ,
5.83532078217670E-294   ,
3.55430312699528E-294   ,
2.16492521102931E-294   ,
1.31865152070052E-294   ,
8.03185559265434E-295   ,
4.89215777148481E-295   ,
2.97977677406127E-295   ,
1.81495441612020E-295   ,
1.10546864732626E-295   ,
6.73326731322039E-296   ,
4.10113441397765E-296   ,
2.49793350127065E-296   ,
1.52144573034142E-296   ,
9.26682122439588E-297   ,
5.64421867220295E-297   ,
3.43776041847181E-297   ,
2.09385263951559E-297   ,
1.27530879134079E-297   ,
7.76753694990386E-298   ,
4.73096830585930E-298   ,
2.88147919009319E-298   ,
1.75501032671408E-298   ,
1.06891360976280E-298   ,
6.51034915167777E-299   ,
3.96519636598744E-299   ,
2.41503742546745E-299   ,
1.47089536620924E-299   ,
8.95856490719259E-300   ,
5.45624498040141E-300   ,
3.32313536472762E-300   ,
2.02395549458404E-300   ,
1.23268650609364E-300   ,
7.50763385954980E-301   ,
4.57248497854965E-301   ,
2.78484002938402E-301   ,
1.69608253618695E-301   ,
1.03298134325685E-301   ,
6.29124706187054E-302   ,
3.83159654136181E-302   ,
2.33357398954470E-302   ,
1.42122275011252E-302   ,
8.65568614293779E-303   ,
5.27156572956552E-303   ,
3.21052849690645E-303   ,
1.95529464781207E-303   ,
1.19082152105353E-303   ,
7.25236946531726E-304   ,
4.41684288491786E-304   ,
2.68994107744464E-304   ,
1.63822055446228E-304   ,
9.97701817885032E-305   ,
6.07614245451566E-305   ,
3.70044474387528E-305   ,
2.25360965201324E-305   ,
1.37246810396174E-305   ,
8.35842834995971E-306   ,
5.09032822626194E-306   ,
3.10002895424484E-306   ,
1.88792403702327E-306   ,
1.14974647210286E-306   ,
7.00194122507590E-307   ,
4.26416148713235E-307   ,
2.59685460508345E-307   ,
1.58146810068839E-307   ,
9.63101479431034E-308   ,
5.86519529161437E-308   ,
3.57183772387752E-308
#endregion	
        };
    }
}
