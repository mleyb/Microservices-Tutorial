using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 <feed xmlns="http://www.w3.org/2005/Atom">
	<id>urn:uuid:95639379-e427-4023-8f03-a183fbd61381</id>
	<title type="text">Index of event stream 95639379-e427-4023-8f03-a183fbd61381</title>
	<updated>2015-06-29T19:04:22.3443078+01:00</updated>
	<author>
		<name>Grean</name>
	</author>
	<link href="028872fe-cad5-492a-822a-1af00ad80708/95639379-e427-4023-8f03-a183fbd61381" rel="self" />
	<entry>
		<id>urn:uuid:505ebbe0-e2a0-4d7f-aab9-fd49b34889b3</id>
		<title type="text">Changeset 505ebbe0-e2a0-4d7f-aab9-fd49b34889b3</title>
		<published>2015-06-29T19:04:22.3443078+01:00</published>
		<updated>2015-06-29T19:04:22.3443078+01:00</updated>
		<author>
			<name>Grean</name>
		</author>
		<content type="application/xml">
		<product-entry xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="urn:paramore:samples:cakeshop">
			<description>Sweet, sweet, almond Cake</description>
			<id>2</id>
			<name>Almond Cake</name>
			<type>Created</type>
			</product-entry>
			</content>
			</entry>
</feed>
 */
using Machine.Specifications;

namespace Store_Core.Adapters.Tests
{
    public class When_reading_an_atom_feed
    {
        private const string atomFeed = " <feed xmlns=\"http://www.w3.org/2005/Atom\">\r\n\t<id>urn:uuid:95639379-e427-4023-8f03-a183fbd61381</id>\r\n\t<title type=\"text\">Index of event stream 95639379-e427-4023-8f03-a183fbd61381</title>\r\n\t<updated>2015-06-29T19:04:22.3443078+01:00</updated>\r\n\t<author>\r\n\t\t<name>Grean</name>\r\n\t</author>\r\n\t<link href=\"028872fe-cad5-492a-822a-1af00ad80708/95639379-e427-4023-8f03-a183fbd61381\" rel=\"self\" />\r\n\t<entry>\r\n\t\t<id>urn:uuid:505ebbe0-e2a0-4d7f-aab9-fd49b34889b3</id>\r\n\t\t<title type=\"text\">Changeset 505ebbe0-e2a0-4d7f-aab9-fd49b34889b3</title>\r\n\t\t<published>2015-06-29T19:04:22.3443078+01:00</published>\r\n\t\t<updated>2015-06-29T19:04:22.3443078+01:00</updated>\r\n\t\t<author>\r\n\t\t\t<name>Grean</name>\r\n\t\t</author>\r\n\t\t<content type=\"application/xml\">\r\n\t\t<product-entry xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:paramore:samples:cakeshop\">\r\n\t\t\t<description>Sweet, sweet, almond Cake</description>\r\n\t\t\t<id>2</id>\r\n\t\t\t<name>Almond Cake</name>\r\n\t\t\t<type>Created</type>\r\n\t\t\t</product-entry>\r\n\t\t\t</content>\r\n\t\t\t</entry>\r\n</feed>";

        //private Because of = () => feedReader.ReadNextItem(atomFeed);
    }
}
